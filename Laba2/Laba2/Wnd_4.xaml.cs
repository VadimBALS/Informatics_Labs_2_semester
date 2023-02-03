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
using System.Windows.Shapes;

namespace Laba2
{
    /// <summary>
    /// Логика взаимодействия для Wnd_4.xaml
    /// </summary>
    public partial class Wnd_4 : Window
    {
        public Wnd_4()
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
        }

        // заполнение кол-вств дней в месяцах
        int[] days = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

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

            // чтоб убрать минусы 
            string result_str = "Befor today " + years + " years " + mnth + " month " + d + " days";
            result_str.Replace('-', ' ');

            // вывод 
            res.Content = result_str;
        }
    }
}
