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

namespace Laba2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void tsk_1_Click(object sender, RoutedEventArgs e)
        {
            // создание объекта Wnd = окно 
            Wnd_1 window = new Wnd_1();
            window.ShowDialog();

        }
        // black
        
        private void tsk_2_Click(object sender, RoutedEventArgs e)
        {
            // создание объекта Wnd = окно 
            Wnd_2 window = new Wnd_2();
            window.ShowDialog();
        }
        // red

        private void tsk_3_Click(object sender, RoutedEventArgs e)
        {
            // создание объекта Wnd = окно 
            Wnd_3 window = new Wnd_3();
            window.ShowDialog();
        }
        // blue
        
        private void tsk_4_Click(object sender, RoutedEventArgs e)
        {
            // создание объекта Wnd = окно 
            Wnd_4 window = new Wnd_4();
            window.ShowDialog();
        }
        // green
        
        private void tsk_5_Click(object sender, RoutedEventArgs e)
        {
            // создание объекта Wnd = окно 
            Wnd_5 window = new Wnd_5();
            window.ShowDialog();
        }
        // yellow

        private void tsk_6_Click(object sender, RoutedEventArgs e)
        {
            // создание объекта Wnd = окно 
            Wnd_6 window = new Wnd_6();
            window.ShowDialog();
        }
        // purpur
    }
}
