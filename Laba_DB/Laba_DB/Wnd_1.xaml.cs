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

namespace Laba_DB
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

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cncl_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
