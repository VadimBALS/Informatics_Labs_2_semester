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

/*
Разработать и реализовать программу “Video Player” на основе компонента MediaElement. 
Программа должна содержать следующий функционал:

    1 Выбор и загрузка видео файла.
    2 Возможность остановить, запустить и поставить на паузу текущий воспроизводимый файл.
    3 Возможность перейти к произвольному моменту воспроизводимого файла при помощи компонента Slider.
    4 Отображение общей длительности воспроизводимого файла и текущего времени воспроизведения.
    5 Возможность регулирования громкости воспроизведения.

Все сообщения обеих программ (ошибки и уведомления), должны сопровождаться звуками.
*/

namespace VideoPlayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
