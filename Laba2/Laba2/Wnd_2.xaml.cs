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
    /// Логика взаимодействия для Wnd_2.xaml
    /// </summary>
    public partial class Wnd_2 : Window
    {
        public Wnd_2()
        {
            InitializeComponent();
        }
        private void add_s_Click(object sender, RoutedEventArgs e)
        {
            res.Content = tb.Text;
        }
    }
}
