using System;
using System.Collections;
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



        //
        int[,] field;

        // size of btn
        int size_btn = 50;

        // размер поля 
        int N = 5;

        // 
        Dictionary<int, int> mines = new Dictionary<int, int>();

        // 
        int countMines;

        // 
        int ost;

        FieldGen gen;

        public MainWindow()
        {
            InitializeComponent();

            dif_var.Items.Add("Baby");
            dif_var.Items.Add("Boy");
            dif_var.Items.Add("Man");

            if (dif_var.SelectedIndex != -1)
            {
                gen = new FieldGen(N, dif_var.SelectedIndex);
                gen.generate();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int n = (int)((Button)sender).Tag;
            ((Button)sender).Background = Brushes.White;
            ((Button)sender).Foreground = Brushes.Black;
            ((Button)sender).FontSize = 20;
            if (mines[n] == 9)
            {
                // создание контейнера под картинку 
                Image img = new Image();
                // записть картинки 
                img.Source = mine;
                // создание компонента для отображения 
                StackPanel stackPnl = new StackPanel();
                // установка толщины границ компонента 
                stackPnl.Margin = new Thickness(1);
                // добавление контейнера с картинкой в компонент 
                stackPnl.Children.Add(img);
                // запись компонента в кнопку 
                ((Button)sender).Content = stackPnl;

                ((Button)sender).IsEnabled = false;

                foreach (Button b in ugr.Children)
                {
                    if (((Button)b).IsEnabled == true)
                    {

                        ((Button)b).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        ost = ost - 1000;
                    }
                }

                if (ost < -1)
                {
                    MessageBox.Show("Поражение\nДля рестарта нажмите кнопку Старт");
                    ost = ost + 999999;
                }
                ((Button)Start).IsEnabled = true;


            }
            else
            {
                ost++;
                ((Button)sender).Content = mines[n];
                //MessageBox.Show(((N * N) - (countMines+1))+" "+ost);
                if (ost == ((N*N) - (countMines + 1)))
                {
                    MessageBox.Show("Победа");
                    ((Button)Start).IsEnabled = true;
                }
            }
            ((Button)sender).IsEnabled = false;

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            ost = 0;
            mines.Clear();
            ugr.Children.Clear();
            N = Convert.ToInt32(Size.Text);

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
                btn.Click += Button_Click;

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


            field = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    field[i, j] = 0;
                }
            }

            AddMines(Convert.ToInt32(CountMinesTxt.Text));

            int index = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (field[i, j] == 9)
                    {
                        mines.Add(index, 9);
                    }
                    else
                    {
                        mines.Add(index, field[i, j]);
                    }
                    index++;
                }
            }
            ((Button)sender).IsEnabled = false;
        }

        void AddMines(int mines)
        {
            countMines = 0;
            for (int i = 0; i < mines; i++)
            {
                Random rand = new Random();
                int _i = rand.Next(0, N);
                int _j = rand.Next(0, N);
                int count = 0;
                if (_i != 0 && _j != 0 && _i != (N - 1) && _j != (N - 1))
                {
                    if (field[_i - 1, _j - 1] == 9 || field[_i, _j - 1] == 9 || field[_i + 1, _j - 1] == 9 || field[_i - 1, _j] == 9 || field[_i + 1, _j] == 9 || field[_i - 1, _j + 1] == 9 || field[_i, _j + 1] == 9 || field[_i + 1, _j + 1] == 9 || field[_i, _j] == 9)
                    {
                        i--;
                        count++;
                        if (count > 25) break;
                    }
                    else
                    {
                        field[_i, _j] = 9;

                        field[_i - 1, _j - 1] += 1;
                        field[_i, _j - 1] += 1;
                        field[_i + 1, _j - 1] += 1;
                        field[_i - 1, _j] += 1;
                        field[_i + 1, _j] += 1;
                        field[_i - 1, _j + 1] += 1;
                        field[_i, _j + 1] += 1;
                        field[_i + 1, _j + 1] += 1;


                        count = 0;
                    }
                }
                else
                {
                    if (_i == 0 && _j == 0)
                    {
                        if (field[_i, _j + 1] == 9 || field[_i + 1, _j] == 9 || field[_i + 1, _j + 1] == 9 || field[_i, _j] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            field[_i, _j] = 9;

                            field[_i, _j + 1] += 1;
                            field[_i + 1, _j] += 1;
                            field[_i + 1, _j + 1] += 1;

                            count = 0;
                        }
                    }
                    else if (_i == 0 && _j != 0 && _j != (N - 1))
                    {
                        if (field[_i, _j - 1] == 9 || field[_i, _j] == 9 || field[_i + 1, _j - 1] == 9 || field[_i + 1, _j] == 9 || field[_i, _j + 1] == 9 || field[_i + 1, _j + 1] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            field[_i, _j] = 9;

                            field[_i, _j - 1] += 1;
                            field[_i + 1, _j - 1] += 1;
                            field[_i + 1, _j] += 1;
                            field[_i, _j + 1] += 1;
                            field[_i + 1, _j + 1] += 1;

                            count = 0;
                        }

                    }
                    else if (_i == 0 && _j == (N - 1))
                    {
                        if (field[_i, _j - 1] == 9 || field[_i + 1, _j] == 9 || field[_i + 1, _j - 1] == 9 || field[_i, _j] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            field[_i, _j] = 9;

                            field[_i, _j - 1] += 1;
                            field[_i + 1, _j] += 1;
                            field[_i + 1, _j - 1] += 1;

                            count = 0;
                        }
                    }
                    else if (_i != 0 && _j == (N - 1) && _i != (N - 1))
                    {
                        if (field[_i - 1, _j - 1] == 9 || field[_i - 1, _j] == 9 || field[_i, _j] == 9 || field[_i + 1, _j] == 9 || field[_i, _j - 1] == 9 || field[_i + 1, _j - 1] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            field[_i, _j] = 9;

                            field[_i - 1, _j - 1] += 1;
                            field[_i - 1, _j] += 1;
                            field[_i + 1, _j] += 1;
                            field[_i, _j - 1] += 1;
                            field[_i + 1, _j - 1] += 1;

                            count = 0;
                        }

                    }
                    else if (_i == (N - 1) && _j == (N - 1))
                    {
                        if (field[_i, _j - 1] == 9 || field[_i, _j] == 9 || field[_i - 1, _j] == 9 || field[_i - 1, _j - 1] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            field[_i, _j] = 9;

                            field[_i, _j - 1] += 1;
                            field[_i - 1, _j] += 1;
                            field[_i - 1, _j - 1] += 1;

                            count = 0;
                        }
                    }
                    else if (_i == (N - 1) && _j != (N - 1) && _j != 0)
                    {
                        if (field[_i - 1, _j - 1] == 9 || field[_i, _j] == 9 || field[_i - 1, _j] == 9 || field[_i - 1, _j + 1] == 9 || field[_i, _j - 1] == 9 || field[_i, _j + 1] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            field[_i, _j] = 9;

                            field[_i - 1, _j - 1] += 1;
                            field[_i - 1, _j] += 1;
                            field[_i - 1, _j + 1] += 1;
                            field[_i, _j - 1] += 1;
                            field[_i, _j + 1] += 1;

                            count = 0;
                        }

                    }
                    else if (_i == (N - 1) && _j == 0)
                    {
                        if (field[_i, _j + 1] == 9 || field[_i, _j] == 9 || field[_i - 1, _j] == 9 || field[_i - 1, _j + 1] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            field[_i, _j] = 9;

                            field[_i, _j + 1] += 1;
                            field[_i - 1, _j] += 1;
                            field[_i - 1, _j + 1] += 1;

                            count = 0;
                        }
                    }

                    else if (_i != (N - 1) && _i != 0 && _j == 0)
                    {
                        if (field[_i - 1, _j + 1] == 9 || field[_i, _j] == 9 || field[_i, _j + 1] == 9 || field[_i + 1, _j + 1] == 9 || field[_i + 1, _j] == 9 || field[_i - 1, _j] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            field[_i, _j] = 9;

                            field[_i - 1, _j + 1] += 1;
                            field[_i, _j + 1] += 1;
                            field[_i + 1, _j + 1] += 1;
                            field[_i + 1, _j] += 1;
                            field[_i - 1, _j] += 1;

                            count = 0;
                        }

                    }

                }
                countMines = i;
            }
            RealCount.Content = (countMines + 1).ToString();
        }





        private void Start_Cl_Click(object sender, RoutedEventArgs e)
        {
            gen = new FieldGen(N, dif_var.SelectedIndex);
            gen.generate();

            ugr.Children.Clear();

            if (dif_var.SelectedIndex == 0)
            {
                N = 4;

                // кол-во строк и столбцов в сетке 
                ugr.Rows = N;
                ugr.Columns = N;

                // размеры сетки = число ячеек * (размер кнопки + толщина границы)
                ugr.Width = N * (size_btn + 4);
                ugr.Height = N * (size_btn + 4);
            }
            if (dif_var.SelectedIndex == 1)
            {
                N = 7;

                // кол-во строк и столбцов в сетке 
                ugr.Rows = N;
                ugr.Columns = N;

                // размеры сетки = число ячеек * (размер кнопки + толщина границы)
                ugr.Width = N * (size_btn + 4);
                ugr.Height = N * (size_btn + 4);
            }
            if (dif_var.SelectedIndex == 2)
            {
                N = 12;

                // кол-во строк и столбцов в сетке 
                ugr.Rows = N;
                ugr.Columns = N;

                // размеры сетки = число ячеек * (размер кнопки + толщина границы)
                ugr.Width = N * (size_btn + 4);
                ugr.Height = N * (size_btn + 4);

            }

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

                MessageBox.Show("  !!!NOW YOU INCUBATOR!!! \n For retry press Start");

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


        }
    }
}
