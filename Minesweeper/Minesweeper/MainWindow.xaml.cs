using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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


namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        // картинки 
        BitmapImage mine  = new BitmapImage(new Uri(@"pack://application:,,,/img/facehugger.jpg", UriKind.Absolute));
        BitmapImage egg   = new BitmapImage(new Uri(@"pack://application:,,,/img/egg_pixelart.png", UriKind.Absolute));
        BitmapImage egg_0 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_0.png", UriKind.Absolute));
        BitmapImage egg_1 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_1.png", UriKind.Absolute));
        BitmapImage egg_2 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_2.png", UriKind.Absolute));
        BitmapImage egg_3 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_3.png", UriKind.Absolute));
        BitmapImage egg_4 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_4.png", UriKind.Absolute));
        BitmapImage egg_5 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_5.png", UriKind.Absolute));
        BitmapImage egg_6 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_6.png", UriKind.Absolute));
        BitmapImage egg_7 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_7.png", UriKind.Absolute));
        BitmapImage egg_8 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_8.png", UriKind.Absolute));

        //звук смерти 
        SoundPlayer death = new SoundPlayer();

        // музыка 
        MediaPlayer player = new MediaPlayer();

        // размер кнопки 
        int size_btn = 50;

        // статусас
        bool fail = false;

        // кол-во мин
        int count_mines;
        
        // размер поля
        int N;
        
        // осталось клеток
        int ost;

        // экземпляр класса 
        FieldGen gen;

        public MainWindow()
        {
            InitializeComponent();

            // добавление уровни сложностей 
            dif_var.Items.Add("Baby");
            dif_var.Items.Add("Boy");
            dif_var.Items.Add("Man");

            //выбор медиа файла, например, в формате .mp3
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            //загрузка выбранного файла
            player.Open(new Uri(dlg.FileName, UriKind.Relative));
            player.Volume = volume.Value;
            player.Play();

        }


        

        private void Start_Cl_Click(object sender, RoutedEventArgs e)
        {
            player.Play();
            // установка статуса 
            fail = false;

            // установка отрицательного доступа к кнопке
            Start_Cl.IsEnabled = false;

            // проверка сложности и вывод кол-ва сгенеренных мин, установка звука смерти,
            // определение кол-ва мин на поле
            if (dif_var.SelectedIndex != -1)
            {
                gen = new FieldGen(dif_var.SelectedIndex);
                gen.generate();

                if (dif_var.SelectedIndex == 0)
                {
                    RealCount.Content = "Generated mines: 3";
                    count_mines = 3;
                    N = 4;
                    ost = 16;
                    death.Stream = Properties.Resources.facehugger_baby;
                }
                else if (dif_var.SelectedIndex == 1)
                {
                    RealCount.Content = "Generated mines: 15";
                    count_mines = 15;
                    N = 7;
                    ost = 49;
                    death.Stream = Properties.Resources.facehugger_boy;
                }
                else
                {
                    RealCount.Content = "Generated mines: 50";
                    count_mines = 50;
                    N = 11;
                    ost = 121;
                    death.Stream = Properties.Resources.facehugger_man;
                }
            }

            // очистка сетки перед новой генерацией
            ugr.Children.Clear();

            // кол-во строк и столбцов в сетке 
            ugr.Rows = N;
            ugr.Columns = N;

            // размеры сетки = число ячеек * (размер кнопки + толщина границы)
            ugr.Width = N * (size_btn + 4);
            ugr.Height = N * (size_btn + 4);
            
            // толщина границ сетки 
            ugr.Margin = new Thickness(5, 5, 5, 5);

            // создание кнопок 
            for (int i = 0; i < N * N; i++)
            {
                // создание кнопки 
                Button btn = new Button();

                // запись номера кнопки 
                btn.Tag = i;

                // установка размеров 
                btn.Width = size_btn;
                btn.Height = size_btn;

                // добавление события 
                btn.Click += Btn_Click;

                // создание контейнера под картинку 
                Image img = new Image();
                // записть картинки 
                img.Source = egg;
                // создание компонента для отображения 
                StackPanel stackPnl = new StackPanel();
                // установка толщины границ компонента 
                stackPnl.Margin = new Thickness(0);
                // добавление контейнера с картинкой в компонент 
                stackPnl.Children.Add(img);
                // запись компонента в кнопку 
                btn.Content = stackPnl;

                // добавление в сетку 
                ugr.Children.Add(btn);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            // получение кнопки 
            int i = (int)((Button)sender).Tag;

            if (gen.getCell(i % N, i / N) == 9)
            {
                // обновление статуса
                fail = true;

                // обновление изображения 
                    // создание контейнера под картинку 
                    Image img = new Image();
                    // записть картинки 
                    img.Source = mine;
                    // создание компонента для отображения 
                    StackPanel stackPnl = new StackPanel();
                    // установка толщины границ компонента 
                    stackPnl.Margin = new Thickness(0);
                    // добавление контейнера с картинкой в компонент 
                    stackPnl.Children.Add(img);
                    // запись компонента в кнопку 
                    ((Button)sender).Content = stackPnl;

                //звук смерти 
                death.Play();

                //уведомление о инкубаторстве
                MessageBox.Show("  !!!NOW YOU INCUBATOR!!! \n For retry press Start");

                //открытие доступа к кнопке
                Start_Cl.IsEnabled = true;
            }
            else
            {
                if (gen.getCell(i % N, i / N) == 0)
                {
                    // создание контейнера под картинку 
                    Image img = new Image();
                    // записть картинки 
                    img.Source = egg_0;
                    // создание компонента для отображения 
                    StackPanel stackPnl = new StackPanel();
                    // установка толщины границ компонента 
                    stackPnl.Margin = new Thickness(0);
                    // добавление контейнера с картинкой в компонент 
                    stackPnl.Children.Add(img);
                    // запись компонента в кнопку 
                    ((Button)sender).Content = stackPnl;
                }
                else if (gen.getCell(i % N, i / N) == 1)
                {
                    // создание контейнера под картинку 
                    Image img = new Image();
                    // записть картинки 
                    img.Source = egg_1;
                    // создание компонента для отображения 
                    StackPanel stackPnl = new StackPanel();
                    // установка толщины границ компонента 
                    stackPnl.Margin = new Thickness(0);
                    // добавление контейнера с картинкой в компонент 
                    stackPnl.Children.Add(img);
                    // запись компонента в кнопку 
                    ((Button)sender).Content = stackPnl;
                }
                else if (gen.getCell(i % N, i / N) == 2)
                {
                    // создание контейнера под картинку 
                    Image img = new Image();
                    // записть картинки 
                    img.Source = egg_2;
                    // создание компонента для отображения 
                    StackPanel stackPnl = new StackPanel();
                    // установка толщины границ компонента 
                    stackPnl.Margin = new Thickness(0);
                    // добавление контейнера с картинкой в компонент 
                    stackPnl.Children.Add(img);
                    // запись компонента в кнопку 
                    ((Button)sender).Content = stackPnl;
                }
                else if (gen.getCell(i % N, i / N) == 3)
                {
                    // создание контейнера под картинку 
                    Image img = new Image();
                    // записть картинки 
                    img.Source = egg_3;
                    // создание компонента для отображения 
                    StackPanel stackPnl = new StackPanel();
                    // установка толщины границ компонента 
                    stackPnl.Margin = new Thickness(0);
                    // добавление контейнера с картинкой в компонент 
                    stackPnl.Children.Add(img);
                    // запись компонента в кнопку 
                    ((Button)sender).Content = stackPnl;
                }
                else if (gen.getCell(i % N, i / N) == 4)
                {
                    // создание контейнера под картинку 
                    Image img = new Image();
                    // записть картинки 
                    img.Source = egg_4;
                    // создание компонента для отображения 
                    StackPanel stackPnl = new StackPanel();
                    // установка толщины границ компонента 
                    stackPnl.Margin = new Thickness(0);
                    // добавление контейнера с картинкой в компонент 
                    stackPnl.Children.Add(img);
                    // запись компонента в кнопку 
                    ((Button)sender).Content = stackPnl;
                }
                else if (gen.getCell(i % N, i / N) == 5)
                {
                    // создание контейнера под картинку 
                    Image img = new Image();
                    // записть картинки 
                    img.Source = egg_5;
                    // создание компонента для отображения 
                    StackPanel stackPnl = new StackPanel();
                    // установка толщины границ компонента 
                    stackPnl.Margin = new Thickness(0);
                    // добавление контейнера с картинкой в компонент 
                    stackPnl.Children.Add(img);
                    // запись компонента в кнопку 
                    ((Button)sender).Content = stackPnl;
                }
                else if (gen.getCell(i % N, i / N) == 6)
                {
                    // создание контейнера под картинку 
                    Image img = new Image();
                    // записть картинки 
                    img.Source = egg_6;
                    // создание компонента для отображения 
                    StackPanel stackPnl = new StackPanel();
                    // установка толщины границ компонента 
                    stackPnl.Margin = new Thickness(0);
                    // добавление контейнера с картинкой в компонент 
                    stackPnl.Children.Add(img);
                    // запись компонента в кнопку 
                    ((Button)sender).Content = stackPnl;
                }
                else if (gen.getCell(i % N, i / N) == 7)
                {
                    // создание контейнера под картинку 
                    Image img = new Image();
                    // записть картинки 
                    img.Source = egg_7;
                    // создание компонента для отображения 
                    StackPanel stackPnl = new StackPanel();
                    // установка толщины границ компонента 
                    stackPnl.Margin = new Thickness(0);
                    // добавление контейнера с картинкой в компонент 
                    stackPnl.Children.Add(img);
                    // запись компонента в кнопку 
                    ((Button)sender).Content = stackPnl;
                }
                else if (gen.getCell(i % N, i / N) == 8)
                {
                    // создание контейнера под картинку 
                    Image img = new Image();
                    // записть картинки 
                    img.Source = egg_8;
                    // создание компонента для отображения 
                    StackPanel stackPnl = new StackPanel();
                    // установка толщины границ компонента 
                    stackPnl.Margin = new Thickness(0);
                    // добавление контейнера с картинкой в компонент 
                    stackPnl.Children.Add(img);
                    // запись компонента в кнопку 
                    ((Button)sender).Content = stackPnl;
                }
            }

            // минус клетка 
            ost -= 1;
        
            if (!fail && ost == count_mines)
            {
                MessageBox.Show("  !!!YOU WIN!!! \n For retry press Start");

                //открытие доступа к кнопке
                Start_Cl.IsEnabled = true;
            }

        }

        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = volume.Value;
        }
    }
}
