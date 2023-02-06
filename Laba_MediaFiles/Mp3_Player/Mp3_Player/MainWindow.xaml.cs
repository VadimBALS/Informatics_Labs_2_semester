using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/*
Необходимо разработать и реализовать программу “Mp3 Player” на основе класса MediaPlayer. Программа должна содержать следующий функционал:

    1 Выбор нескольких аудио файлов в формате mp3.                      1 YES
    2 Отображение названий выбранных файлов в компоненте ListBox.       2 YES
    3 Выбор и воспроизведение файла из компонента ListBox.              3 YES
    4 Последовательное воспроизведение файлов из компонента ListBox.    4 YES
    5 Воспроизведение файлов из компонента ListBox в случайном порядке. 5 NO
    6 Возможность остановить, запустить и поставить на                  6 YES
      паузу текущий воспроизводимый файл.
    7 Возможность перейти к произвольному моменту воспроизводимого      7 YES
      файла при помощи компонента Slider.
    8 Отображение общей длительности воспроизводимого файла и           8 YES
      текущего времени воспроизведения.
    9 Возможность регулирования громкости воспроизведения.              9 YES

Все сообщения обеих программ (ошибки и уведомления), должны сопровождаться звуками. NO
*/

//подключение пространства имён
using System.Media;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace Mp3_Player
{
    public partial class MainWindow : Window
    {
        //создание объекта, обычно глобального
        MediaPlayer player = new MediaPlayer();
        
        // создание таймера для отслеживания произведения трека 
        DispatcherTimer dt = new DispatcherTimer();

        // для определения обновления трека при перетаскивании 
        bool uptd = false;

        // словарь для хранения имени трека и пути к нему
        Dictionary<string, string> plist= new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();

            // прячу кнопку 
            //btn_baka.Visibility = Visibility.Hidden;

            // установка громкости 
            player.Volume = volume.Value;

            // привязка реакции на запуск трека  
            player.MediaOpened += Player_MediaOpened;

            // привязка реакции на окончание трека 
            player.MediaEnded += Player_MediaEnded;

            // определение интервала таймера в 1 секунду 
            dt.Interval = new TimeSpan(0,0,1);

            // привязка метода вызываемого раз в секунду для отслеживания момента трека 
            dt.Tick += Dt_Tick;
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            // автообновление позиции ползунка 
            if (uptd == false)
                progress_bar.Value = player.Position.Ticks ;

            // на какой щас секунде 
            now_moment.Content = player.Position.ToString().Substring(0, 8);

        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            //выбор медиа файлoв
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.ShowDialog();

            //загрузка всех выбранных файлов в словарь и листбокс  
            foreach (string filename in dlg.FileNames)
            {
                // созданеие буферной переменной для проверки 
                Dictionary<string, string> buf = new Dictionary<string, string>();
                buf.Add(System.IO.Path.GetFileName(filename), filename);

                try
                {
                    // если такого имени ещё не было - добавляем 
                    if (!plist.ContainsKey(System.IO.Path.GetFileName(filename)))
                    {
                        plist.Add(System.IO.Path.GetFileName(filename), filename);
                        playlist.Items.Add(System.IO.Path.GetFileName(filename));
                    }
                }
                catch (Exception ex) //если возникла ошибка, вывести сообщение об ошибке
                {
                    MessageBox.Show(ex.ToString());

                    // Oops
                    SystemSounds.Hand.Play();
                }

            }
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            // остановка таймера 
            dt.Stop();

            // след трек или первый  
            playlist.SelectedIndex = (playlist.SelectedIndex + 1) % playlist.Items.Count;

            // воспроизведение
            player.Play();

            // запуск таймера 
            dt.Start();
        }

        private void Player_MediaOpened(object sender, EventArgs e)
        {
            try
            {
                // установка максимума ползунка 
                progress_bar.Maximum = player.NaturalDuration.TimeSpan.Ticks;

                // установка времени скока всего идёт трек
                dur.Content = player.NaturalDuration.TimeSpan.ToString().Substring(0, 8);

                // вывод имени трека 
                tr_name.Content = System.IO.Path.GetFileName(plist[playlist.SelectedItem.ToString()]);

                now_moment.Content = "00:00:00";
            }
            catch (Exception ex) //если возникла ошибка, вывести сообщение об ошибке
            {
                player.Stop();

                // Oops
                SoundPlayer sp = new SoundPlayer();
                sp.Stream = Properties.Resources.BAKA;
                sp.Play();

                MessageBox.Show(ex.ToString());
            }
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            //// воспроизведение
            player.Play();

            // запуск таймера 
            dt.Start();
        }

        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // громкость звука = значению громкости
            player.Volume = volume.Value;
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            // остановка воспроизведения 
            player.Pause();

            //остановка тймера
            dt.Stop();
        }

        private void progress_bar_ValueChanged(object sender, DragCompletedEventArgs e)
        {
            // изменение момента трека ручками 
            player.Position = new TimeSpan((long)progress_bar.Value);

            // разблокировка автообновления похиции ползунка 
            uptd = false;
        }

        private void progress_bar_DragStarted(object sender, DragStartedEventArgs e)
        {
            // блокировка автообновления позиции ползунка 
            // на время перетаскивания 
            uptd = true;
        }

        private void playlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //загрузка выбранного файла
            if (playlist.SelectedIndex > -1) 
            { 
                player.Open(new Uri(plist[playlist.SelectedItem.ToString()], UriKind.Relative));
            }

            // обнуление имени  трека
            tr_name.Content = "";

            // обнуление общей продолжительности трека 
            dur.Content = "";

            // обнуление момента 
            now_moment.Content = "";
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            // след трек или первый  
            playlist.SelectedIndex = (playlist.SelectedIndex + 1) % playlist.Items.Count;

            // вывод имени трека 
            tr_name.Content = System.IO.Path.GetFileName(plist[playlist.SelectedItem.ToString()]);

            // воспроизведение
            player.Play();

            // запуск таймера 
            dt.Start();
        }
    }
}
