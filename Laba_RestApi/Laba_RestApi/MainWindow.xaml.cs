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
Необходимо разработать и реализовать программное средство, состоящее и двух приложений:
    Сервер.
    Клиент.
Приложения должны иметь WPF интерфейс. Сервер должен поддерживать подключение более двух клиентов.

Алгоритм работы программного средства:
    Запуск сервера.
    Запуск клиента.
    Указание имени пользователя и ip адреса сервера.
    Подключение к серверу.
    Отправка сообщения на сервер.
    Сервер получает сообщение, переписывает задом наперёд и отправляет обратно.
    Клиент получает инвертированное сообщение от сервера.
    Повторение шага 5 пока не надоест.
    Отключение от сервера.
*/

namespace Laba_RestApi
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
