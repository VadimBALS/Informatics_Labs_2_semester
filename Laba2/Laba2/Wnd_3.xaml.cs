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
    /// Логика взаимодействия для Wnd_3.xaml
    /// </summary>
    public partial class Wnd_3 : Window
    {
        public Wnd_3()
        {
            InitializeComponent();

            // заполнение листа планетами 
            ltbx.Items.Add("Меркурий");
            ltbx.Items.Add("Венера");
            ltbx.Items.Add("Земля");
            ltbx.Items.Add("Марс");
            ltbx.Items.Add("Юпитер");
            ltbx.Items.Add("Сатурн");
            ltbx.Items.Add("Уран");
            ltbx.Items.Add("Нептун");
        }

        // заполнение массива с инфой о планетах
        string[] infa = {"Мерку́рий — наименьшая планета Солнечной системы и самая близкая к Солнцу.",
                        "Вене́ра — вторая по удалённости от Солнца и шестая по размеру планета Солнечной системы",
                        "Земля́ — третья по удалённости от Солнца планета Солнечной системы.",
                        "Марс — четвёртая по удалённости от Солнца",
                        "Юпитер - инфомация о юпитере",
                        "Сатурн - инфомация о сатурне",
                        "Уран - инфомация о уране",
                        "Нептун - инфомация о нептуне"};

        private void ltbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ltbx.SelectedIndex > -1)
            {
                tb.Text = infa[ltbx.SelectedIndex];
            }
        }
    }
}
