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

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[,] field;
        int size;
        Dictionary<int, int> mines = new Dictionary<int, int>();
        BitmapImage mine;
        int countMines;
        int ost;

        public MainWindow()
        {
            InitializeComponent();

            mine = new BitmapImage(new Uri(@"pack://application:,,,/images/mine.jpg", UriKind.Absolute));
        }


        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            int n = (int)((Button)sender).Tag;
            ((Button)sender).Background = Brushes.White;
            ((Button)sender).Foreground = Brushes.Red;
            ((Button)sender).FontSize = 23;
            if (mines[n] == 9)
            {
                
                Image img = new Image();
                img.Source = mine;
                StackPanel stackPnl = new StackPanel();
                stackPnl.Margin = new Thickness(1);
                stackPnl.Children.Add(img);
                ((Button)sender).Content = stackPnl;
                ((Button)sender).IsEnabled = false;
                
                foreach (Button b in Uniform.Children)
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
                //MessageBox.Show(((size * size) - (countMines+1))+" "+ost);
                if (ost==((size*size)-(countMines+1)))
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
            Uniform.Children.Clear();
            size = Convert.ToInt32(Size.Text);

            Uniform.Rows = size;
            Uniform.Columns = size;

            for (int i = 0; i < size * size; i++)
            {
                Button btn = new Button();
                btn.Background = Brushes.Pink;
                btn.Content = "";
                btn.Tag = i;
                btn.Width = 50*size;
                btn.Height = 50*size;
                btn.Content = " ";
                //btn.Margin = new Thickness(2);
                btn.Click += Button_Click;
                Uniform.Children.Add(btn);
            }


            field = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    field[i, j] = 0;
                }
            }

            AddMines(Convert.ToInt32(CountMinesTxt.Text));

            int index = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if(field[i,j]==9)
                    {
                        mines.Add(index, 9);
                    }
                    else
                    {
                        mines.Add(index, field[i,j]);
                    }
                    index++;
                }
            }
            ((Button)sender).IsEnabled = false;
        }

        void AddMines(int mines)
        {
            countMines=0;
            for (int i = 0; i < mines; i++)
            {
                Random rand = new Random();
                int _i = rand.Next(0, size);
                int _j = rand.Next(0, size);
                int count = 0;
                if (_i != 0 &&  _j != 0 && _i!=(size-1) && _j!= (size - 1))
                {
                    if (field[_i - 1, _j - 1] == 9 || field[_i, _j - 1] == 9 || field[_i + 1, _j - 1] == 9 || field[_i - 1, _j] == 9 || field[_i + 1, _j] == 9 || field[_i - 1, _j + 1] == 9 || field[_i, _j + 1] == 9 || field[_i + 1, _j + 1] == 9 || field[_i,_j]==9)
                    {
                        i--;
                        count++;
                        if (count > 25) break;
                    }
                    else
                    {
                        //MessageBox.Show(""); //сделать дебаг везде
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
                    if(_i==0 &&_j==0)
                    {
                        if(field[_i,_j+1]==9 || field[_i+1,_j]==9 || field[_i+1,_j+1]==9 || field[_i, _j] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            //MessageBox.Show(""); //сделать дебаг везде
                            field[_i, _j] = 9;

                            field[_i, _j + 1] += 1;
                            field[_i + 1, _j] += 1;
                            field[_i + 1, _j + 1] += 1;

                            count = 0;
                        }
                    }
                    else if(_i==0 && _j!=0 && _j!= (size - 1))
                    {
                        if(field[_i, _j - 1] == 9 || field[_i, _j] == 9 || field[_i + 1, _j - 1] == 9 || field[_i + 1, _j] == 9 || field[_i, _j + 1] == 9 || field[_i+1,_j+1]==9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            //MessageBox.Show(""); //сделать дебаг везде
                            field[_i, _j] = 9;

                            field[_i, _j - 1] += 1;
                            field[_i + 1, _j - 1] += 1;
                            field[_i + 1, _j] += 1;
                            field[_i, _j + 1] += 1;
                            field[_i + 1, _j + 1] += 1;

                            count = 0;
                        }

                    }
                    else if(_i==0 && _j== (size - 1))
                    {
                        if (field[_i, _j - 1] == 9 || field[_i + 1, _j] == 9 || field[_i + 1, _j - 1] == 9 || field[_i, _j] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            //MessageBox.Show(""); //сделать дебаг везде
                            field[_i, _j] = 9;

                            field[_i, _j - 1] += 1;
                            field[_i + 1, _j] += 1;
                            field[_i + 1, _j - 1] += 1;

                            count = 0;
                        }
                    }
                    else if (_i != 0 && _j == (size - 1) && _i != (size-  1))
                    {
                        if (field[_i-1, _j - 1] == 9 || field[_i - 1, _j] == 9 || field[_i, _j] == 9 || field[_i + 1, _j] == 9 || field[_i, _j - 1] == 9 || field[_i + 1, _j - 1] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            //-MessageBox.Show(""); //сделать дебаг везде
                            field[_i, _j] = 9;

                            field[_i - 1, _j - 1] += 1;
                            field[_i - 1, _j] += 1;
                            field[_i + 1, _j] += 1;
                            field[_i, _j - 1] += 1;
                            field[_i + 1, _j - 1] += 1;

                            count = 0;
                        }

                    }
                    else if (_i == (size-1) && _j == (size - 1))
                    {
                        if (field[_i, _j - 1] == 9 || field[_i, _j] == 9 || field[_i - 1, _j] == 9 || field[_i - 1, _j - 1] == 9)
                        {
                            //MessageBox.Show(""); //сделать дебаг везде
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            //MessageBox.Show(""); //сделать дебаг везде
                            field[_i, _j] = 9;

                            field[_i, _j - 1] += 1;
                            field[_i - 1, _j] += 1;
                            field[_i - 1, _j - 1] += 1;

                            count = 0;
                        }
                    }
                    else if (_i == (size-1) && _j != (size - 1) && _j!=0)
                    {
                        if (field[_i - 1, _j - 1] == 9 || field[_i, _j] == 9 || field[_i - 1, _j] == 9 || field[_i - 1, _j+1] == 9 || field[_i, _j - 1] == 9 || field[_i, _j + 1] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            //MessageBox.Show(""); //сделать дебаг везде
                            field[_i, _j] = 9;

                            field[_i - 1, _j - 1] += 1;
                            field[_i - 1, _j] += 1;
                            field[_i - 1, _j + 1] += 1;
                            field[_i, _j - 1] += 1;
                            field[_i, _j + 1] += 1;

                            count = 0;
                        }

                    }
                    else if (_i == (size - 1) && _j == 0)
                    {
                        if (field[_i, _j + 1] == 9 || field[_i, _j] == 9 || field[_i - 1, _j] == 9 || field[_i - 1, _j + 1] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            //MessageBox.Show(""); //сделать дебаг везде
                            field[_i, _j] = 9;

                            field[_i, _j + 1] += 1;
                            field[_i - 1, _j] += 1;
                            field[_i - 1, _j + 1] += 1;

                            count = 0;
                        }
                    }

                    else if (_i != (size - 1) && _i != 0 && _j == 0)
                    {
                        if (field[_i - 1, _j + 1] == 9 || field[_i, _j] == 9 || field[_i, _j+1] == 9 || field[_i + 1, _j + 1] == 9 || field[_i+1, _j] == 9 || field[_i-1, _j] == 9)
                        {
                            i--;
                            count++;
                            if (count > 25) break;
                        }
                        else
                        {
                            //MessageBox.Show(""); //сделать дебаг везде
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

        private void Size_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
