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
    /// Логика взаимодействия для Wnd_1.xaml
    /// </summary>
    public partial class Wnd_1 : Window
    {
        public Wnd_1()
        {
            InitializeComponent();
        }
        private void sum_Click(object sender, RoutedEventArgs e)
        {
            float a = float.Parse(tb_a.Text);
            float b = float.Parse(tb_b.Text);

            res.Content = a + b;
        }

        private void sub_Click(object sender, RoutedEventArgs e)
        {
            float a = float.Parse(tb_a.Text);
            float b = float.Parse(tb_b.Text);

            res.Content = a - b;
        }

        private void mul_Click(object sender, RoutedEventArgs e)
        {
            float a = float.Parse(tb_a.Text);
            float b = float.Parse(tb_b.Text);

            res.Content = a * b;
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            float a = float.Parse(tb_a.Text);
            float b = float.Parse(tb_b.Text);

            if (b != 0)
            {
                res.Content = a / b;
            }
            else
                res.Content = "ne nado tak";
        }
    }
}
