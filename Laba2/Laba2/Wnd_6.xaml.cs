using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;

namespace Laba2
{
    public partial class Wnd_6 : Window
    {
        public Wnd_6()
        {
            InitializeComponent();
        }
        private async void op_Click(object sender, RoutedEventArgs e)
        {
            // спрашивание пути к файлу 
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            // запись пути к файлу 
            string path = dlg.FileName;

            // переменная buffer 
            string buf = "";
            
            // чтение в буфер 
            using (StreamReader reader = new StreamReader(path))
                buf = await reader.ReadToEndAsync();

            // вывод 
            tb.Text = buf;
        }

        private async void sv_Click(object sender, RoutedEventArgs e)
        {
            // спрашивание пути к файлу 
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            // запись пути к файлу 
            string path = dlg.FileName;

            // запись в буфер из TextBox
            string buf = tb.Text;

            // запись в файл 
            using (StreamWriter writer = new StreamWriter(path, false))
                await writer.WriteLineAsync(buf);
        }
    }
}
