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
Разработать программное приложение WPF, имеющее графический интерфейс и реализующее
следующие функции:
    1) Ввод данных о студентах: уникальный номер, фио, оценка по физике, оценка по
математике.
    2) Добавление данных в таблицу базы данных SQLite через интерфейс приложения.
    3) Чтение данных из базы данных SQLite и отображение их на форме приложения.
    4) Редактирование данных в базе данных SQLite через интерфейс приложения.
    5) Удаление данных из таблиц

База данных должна содержать 2 таблицы, связанные через уникальный номер:
    1. Таблица, содержащая уникальный номер и фио.
    2. Таблица, содержащая уникальный номер и оценки.   
 */

namespace Laba_DB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
