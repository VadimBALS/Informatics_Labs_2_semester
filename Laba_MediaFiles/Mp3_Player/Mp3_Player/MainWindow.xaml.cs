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
Необходимо разработать и реализовать программу “Mp3 Player” на основе класса MediaPlayer. Программа должна содержать следующий функционал:

    1 Выбор нескольких аудио файлов в формате mp3.
    2 Отображение названий выбранных файлов в компоненте ListBox.
    3 Выбор и воспроизведение файла из компонента ListBox.
    4 Последовательное воспроизведение файлов из компонента ListBox.
    5 Воспроизведение файлов из компонента ListBox в случайном порядке.
    6 Возможность остановить, запустить и поставить на паузу текущий воспроизводимый файл.
    7 Возможность перейти к произвольному моменту воспроизводимого файла при помощи компонента Slider.
    8 Отображение общей длительности воспроизводимого файла и текущего времени воспроизведения.
    9 Возможность регулирования громкости воспроизведения.
*/

namespace Mp3_Player
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
