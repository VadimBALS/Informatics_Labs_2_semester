using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/*
Разработать программное приложение, имеющее графический интерфейс и реализующее следующие
функции:
    1. Добавление “Таймера”. Под добавлением таймера подразумевается ввод или выбор
пользователем даты, до которой требуется отсчитывать время, а также, ввод имени,
ассоциированного с этим таймером. Добавленные таймеры должны отображаться на форме
приложения в виде списка.                                                               1 YES
    2. Выбор таймера из списка и отображение времени, оставшегося до наступления
указанной в нём даты, в формате: дни / часы / минуты / секунды                          2 YES
    3. Возможность редактирования и удаления таймеров из списка.                        3 NO
    4. Возможность сохранения списка таймеров в текстовый файл.                         4 YES
    5. Возможность загрузки списка таймеров из текстового файла.                        5 NO

Дополнительно:
    1. Добавление таймера должно выполняться при помощи диалогового окна.               1 YES
    2. Возможность выбрать формат отображения оставшегося времени:                      2 YES
            дни / часы / минуты / секунды
            часы / минуты / секунды(сколько всего часов минут и секунд осталось)
            минуты / секунды(сколько всего минут и секунд осталось)
            секунды(сколько всего секунд осталось)
    3. Возможность свернуть программу в область уведомлений                             3 YES
*/

namespace Laba_Timer
{
    public partial class MainWindow : Window
    {
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
        public MainWindow()
        {
            InitializeComponent();
            
            // заполнение с 1990 до 2023 года 
            for (int i = 1990; i < 2023; i++)
                year.Items.Add(i);

            // заполнение месяцев
            month.Items.Add("January");
            month.Items.Add("February");
            month.Items.Add("March");
            month.Items.Add("April");
            month.Items.Add("May");
            month.Items.Add("June"); 
            month.Items.Add("July");
            month.Items.Add("August");
            month.Items.Add("September");
            month.Items.Add("October");
            month.Items.Add("November");
            month.Items.Add("December");

            // заполнение форматов 
            ft.Items.Add("days/hours/min/sec");
            ft.Items.Add("hours/min/sec");
            ft.Items.Add("min/sec");
            ft.Items.Add("sec");

            //загрузка картинки, которая будет отображаться в области уведомлений
            ni.Icon = new System.Drawing.Icon("Timer.ico");
            ni.Visible = true;
            //обработчик события "двойной клик" по иконке в области уведомлений
            ni.DoubleClick +=
            delegate (object sender, EventArgs args)
            {
                //показать окно
                this.Show();
                this.WindowState = WindowState.Normal;
            };
        }

        protected override void OnStateChanged(EventArgs e)
        {
            //вместо того что бы сворачивать окно, обработчик события будет его скрывать
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();
            base.OnStateChanged(e);
        }
        //перезапись метода обработки события сворачивания окна
        
        
        // заполнение кол-вств дней в месяцах
        int[] days = new int[] {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

        private void year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // проверка високосности года  
            if ((int)year.SelectedItem % 4 == 0)
                days[1] = 29;
            else
                days[1] = 28;

            if (year.SelectedIndex > -1 && month.SelectedIndex > -1)
            {
                // открытие выбора дней
                day.IsEnabled = true;

                // очистка массива с днями для перегенерации
                day.Items.Clear();

                // заполнение дней 
                for (int i = 1; i <= days[month.SelectedIndex]; i++)
                    day.Items.Add(i);
            }
        }
        // годы 

        private void month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (year.SelectedIndex > -1 && month.SelectedIndex > -1)
            {
                // открытие выбора дней
                day.IsEnabled = true;

                // заполнение дней
                for (int i = 1; i <= days[month.SelectedIndex]; i++)
                    day.Items.Add(i);
            }
        }
        // месяцы 

        private void day_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // создание переменной типа дататайм с введёнными значениями    
            DateTime dt = new DateTime();
            
            if (year.SelectedIndex > -1 && month.SelectedIndex > -1 && day.SelectedIndex > -1) 
                dt = new DateTime((int)year.SelectedItem, month.SelectedIndex + 1, (int)day.SelectedItem);
            //                                                                + 1 тк месяцы начинаются с 0

            // вычисление промежутка между 2 датами: выбранной и сегодняшней
            TimeSpan ts = DateTime.Now.Subtract(dt);

            // перевод промежутка в нормальный вид 
            DateTime result = new DateTime();
            result = result.AddSeconds(ts.TotalSeconds);

            // -1 тк при создании dt переменная равна 01.01.0001
            result = result.AddYears(-1);
            int years = int.Parse(result.ToString("yyyy"));
            int mnth = int.Parse(result.ToString("MM")) - 1;
            int d = int.Parse(result.ToString("dd")) - 1;

            // вывод 
            res.Content = "Befor today " + years + " years " + mnth + " month " + d + " days";
        }
        // дни


        // создание словаря с таймерами 
        Dictionary<string, DateTime> timers = new Dictionary<string, DateTime>();
        
        private void Button_Click_add_timer(object sender, RoutedEventArgs e)
        {
            // создание объекта Wnd = окно 
            Wnd window = new Wnd();

            // если согласились (ок)
            if (window.ShowDialog() == true)
            {
                // взятие данных из календаря 
                int year = window.clndr.SelectedDate.Value.Year;
                int month = window.clndr.SelectedDate.Value.Month;
                int day = window.clndr.SelectedDate.Value.Day;

                // взятие данных из полей
                int hour = int.Parse(window.hour.Text);
                int min = int.Parse(window.min.Text);
                int sec = int.Parse(window.seconds.Text);

                // создание объекта datetime с взятыми параметрами 
                DateTime dt = new DateTime(year, month, day, hour, min, sec);

                // добавление нового таймера если такого ключа ещё не было
                if (timers.ContainsKey(window.nm_tmr.Text) == false)
                {
                    timers.Add(window.nm_tmr.Text, dt);
                    tm_lst.Items.Add(window.nm_tmr.Text);
                }
                else
                    System.Windows.MessageBox.Show("Name already exists");


                lb.Content = dt.ToString();
            }
            else
                lb.Content = "Canceled";
        }

        private void tm_lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tm_lst.SelectedIndex > -1)
            {
                // включение выбора форматов
                ft.IsEnabled = true;

                // вывод значения таймера по ключу 
                lb.Content = tm_lst.SelectedItem.ToString() + " " + timers[tm_lst.SelectedItem.ToString()].ToString();
            }
        }

        private void ft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tm_lst.SelectedIndex > -1)
            {
                // вычисление промежутка между 2 датами: выбранной и сегодняшней
                TimeSpan ts = DateTime.Now.Subtract(timers[tm_lst.SelectedItem.ToString()]);

                if (ft.SelectedIndex == 0) // days/hours/min/sec
                {
                    bfr_td.Content = "Befor today " + (int)ts.TotalSeconds / 60 / 60 / 24 + " days "
                                                    + (int)ts.TotalSeconds / 60 / 60 % 24 + " hours " 
                                                    + (int)ts.TotalSeconds / 60 % 60+ " min " 
                                                    + (int)ts.TotalSeconds % 60 + " sec";
                }
                if (ft.SelectedIndex == 1) // hours/min/sec
                {
                    bfr_td.Content = "Befor today " + (int)ts.TotalSeconds / 60 / 60 + " hours " + 
                                                    + (int)ts.TotalSeconds / 60 % 60 + " min " 
                                                    + (int)ts.TotalSeconds % 60 + " sec";
                }
                if (ft.SelectedIndex == 2) // min/sec
                {
                    bfr_td.Content = "Befor today " + (int)ts.TotalSeconds / 60 + " min " 
                                                    + (int)ts.TotalSeconds % 60 + " sec";
                }
                if (ft.SelectedIndex == 3) // sec
                {
                    bfr_td.Content = "Befor today " + (int)ts.TotalSeconds + " seconds";
                }
             }
        }

        private async void Button_Click_save_list(object sender, RoutedEventArgs e)
        {
            // спрашивание пути к файлу 
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            // запись пути к файлу 
            string path = dlg.FileName;

            // переменная для хранения таймеров 
            string timers_list = ""; 

            //запись таймеров 
            foreach(var time in timers)
                timers_list += time.Key + " " + time.Value + "\n";

            // запись в файл 
            using (StreamWriter writer = new StreamWriter(path, false))
                await writer.WriteLineAsync(timers_list);
        }
    }
}
