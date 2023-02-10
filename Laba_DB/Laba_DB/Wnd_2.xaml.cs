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
    public partial class Wnd_2 : Window
    {
        public Wnd_2()
        {
            InitializeComponent();
        }
        public Wnd_2(string uname, int g_m, int g_p)
        {
            InitializeComponent();

            name.Text = uname;
            g_math.Text = g_m.ToString();
            g_phys.Text = g_p.ToString();
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
