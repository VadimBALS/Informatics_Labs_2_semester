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
    public partial class Wnd_5 : Window
    {
        // создание переменной Таймер
        System.Windows.Threading.DispatcherTimer Timer;

        
        DateTime start_timer = new DateTime();
        DateTime now_timer = new DateTime();


        public Wnd_5()
        {
            InitializeComponent();

            //инициализация переменной таймер
            Timer = new System.Windows.Threading.DispatcherTimer();

            //назначение обработчика события Тик
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
            
            //установка интервала между тиками
            //TimeSpan – переменная для хранения времени в формате часы/минуты/секунды
            Timer.Interval = new TimeSpan(0, 0, 1);
        }
        
        
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            now_timer = DateTime.Now;
            TimeSpan r = now_timer - start_timer;

            if (ch_b.IsChecked != true)
                time.Content = (int)r.TotalSeconds;
            else
                time.Content = r.ToString(@"hh\:mm\:ss");
        }
        //обработчик события Тик
        
        private void start_Click(object sender, RoutedEventArgs e)
        {
            start_timer = DateTime.Now;
            Timer.Start();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            lb.Items.Add(time.Content);
        }

        private void drop_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            time.Content = "";  
        }
    }
}
