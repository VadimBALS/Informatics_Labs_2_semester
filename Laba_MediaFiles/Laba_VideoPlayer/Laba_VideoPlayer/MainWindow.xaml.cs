using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

/*
Разработать и реализовать программу “Video Player” на основе компонента MediaElement. 
Программа должна содержать следующий функционал:

    1 Выбор и загрузка видео файла.
    2 Возможность остановить, запустить и поставить на паузу текущий воспроизводимый файл.
    3 Возможность перейти к произвольному моменту воспроизводимого файла при помощи компонента Slider.
    4 Отображение общей длительности воспроизводимого файла и текущего времени воспроизведения.
    5 Возможность регулирования громкости воспроизведения.

Все сообщения обеих программ (ошибки и уведомления), должны сопровождаться звуками.
*/

//подключение пространства имён
using System.Media;

namespace Laba_VideoPlayer
{
    public partial class MainWindow : Window
    {
        //создание объекта, обычно глобального
        //MediaElement player = new MediaElement();

        // создание таймера для отслеживания произведения трека 
        DispatcherTimer dt = new DispatcherTimer();

        // для определения обновления трека при перетаскивании 
        bool uptd = false;
        public MainWindow()
        {
            InitializeComponent();

            // установка громкости 
            vplayer.Volume = volume.Value;

            // привязка реакции на запуск фильма 
            vplayer.MediaOpened += Player_MediaOpened;

            // привязка реакции на окончание фильма
            vplayer.MediaEnded += Player_MediaEnded;

            // определение интервала таймера в 1 секунду 
            dt.Interval = new TimeSpan(0, 0, 1);

            // привязка метода вызываемого раз в секунду для отслеживания момента трека 
            dt.Tick += Dt_Tick;
        }
        private void Dt_Tick(object sender, EventArgs e)
        {
            // автообновление позиции ползунка 
            if (uptd == false)
                progress_bar.Value = vplayer.Position.Ticks;

            // на какой щас секунде 
            now_moment.Content = vplayer.Position.ToString().Substring(0, 8);

        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            //выбор медиа файлa
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            //установка источника
            vplayer.Source = new Uri(dlg.FileName, UriKind.Relative);

            // обнуление общей продолжительности 
            dur.Content = "";

            // обнуление момента  
            now_moment.Content = "";

            // старт таймера
            dt.Start();
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            // остановка таймера 
            dt.Stop();
        }

        private void Player_MediaOpened(object sender, EventArgs e)
        {
            try
            {
                // установка максимума ползунка 
                progress_bar.Maximum = vplayer.NaturalDuration.TimeSpan.Ticks;

                // установка времени скока всего идёт трек
                dur.Content = vplayer.NaturalDuration.TimeSpan.ToString().Substring(0, 8);

                now_moment.Content = "00:00:00";
            }
            catch (Exception ex) //если возникла ошибка, вывести сообщение об ошибке
            {
                vplayer.Stop();

                // Oops
                SoundPlayer sp = new SoundPlayer();
                sp.Stream = Properties.Resources.baka;
                sp.Play();

                MessageBox.Show(ex.ToString());
            }
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            // воспроизведение
            vplayer.Play();

            // запуск таймера 
            dt.Start();
        }

        //private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    // громкость звука = значению громкости
        //    vplayer.Volume = volume.Value;
        //}

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            // остановка воспроизведения 
            vplayer.Pause();

            //остановка тймера
            dt.Stop();
        }

        private void progress_bar_ValueChanged(object sender, DragCompletedEventArgs e)
        {
            // изменение момента трека ручками 
            vplayer.Position = new TimeSpan((long)progress_bar.Value);

            // разблокировка автообновления похиции ползунка 
            uptd = false;
        }

        private void progress_bar_DragStarted(object sender, DragStartedEventArgs e)
        {
            // блокировка автообновления позиции ползунка 
            // на время перетаскивания 
            uptd = true;
        }


    }
}
