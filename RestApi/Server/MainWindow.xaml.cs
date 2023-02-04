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
    Разработать программное средство, состоящее и двух приложений:
        1) Сервер.
        2) Клиент.
    
    Приложения должны иметь WPF интерфейс. Сервер должен поддерживать подключение более двух
    клиентов.
        Алгоритм работы программного средства:
        1) Запуск сервера.                                                              1 YES
        2) Запуск клиента.                                                              2 YES
        3) Указание имени пользователя.                                                 3 YES
        4) Подключение к серверу.                                                       4 YES
        5) Отправка сообщения на сервер.                                                5 YES
        6) Сервер получает сообщение, переписывает задом наперёд и отправляет обратно.  6 
        7) Клиент получает инвертированное сообщение от сервера.                        7 
        8) Повторение шага 5 пока не надоест.                                           8 
        9) Отключение от сервера.                                                       9 
    
Дополнительное задание: реализовать обмен сообщениями между клиентами, подключёнными к
    серверу.
*/




//подключение библиотек для работы с сетью и потоками
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public partial class MainWindow : Window
    {
        //прослушиваемый порт
        int port = 8888;
        //объект, прослушивающий порт
        static TcpListener listener;

        public MainWindow()
        {
            InitializeComponent();
        }

        //функция ожидания и приёма запросов на подключение
        void listen()
        {
            //цикл подключения клиентов
            while (true)
            {
                //принятие запроса на подключение
                TcpClient client = listener.AcceptTcpClient();
                //log.Items.Add("Новый клиент подключен.");
                

                //создание нового потока для обслуживания нового клиента
                Thread clientThread = new Thread(() => Process(client));
                clientThread.Start();

            }
        }


        private void stert_server_Click(object sender, RoutedEventArgs e)
        {
            //создание объекта для отслеживания сообщений переданных с ip адреса через порт
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            //начало прослушивания
            listener.Start();

            // сообщение о запуске сервера  
            log.Items.Add("Сервер запущен");

            //создание нового потока для ожидания и подключения клиентов
            Thread listenThread = new Thread(() => listen()); // лямбда функция  
            listenThread.Start();

        }

        //обработка сообщений от клиента
        public void Process(TcpClient tcpClient)
        {
            TcpClient client = tcpClient;
            NetworkStream stream = null; //получение канала связи с клиентом

            try //означает что в случае возникновении ошибки, управление перейдёт к блоку catch
            {
                //получение потока для обмена сообщениями
                stream = client.GetStream(); //получение канала связи с клиентом
                                             // буфер для получаемых данных

                Dispatcher.BeginInvoke(new Action(() => log.Items.Add("Новый клиент подключен.")));
                
                byte[] data = new byte[64];
                //цикл ожидания и отправки сообщений
                while (true)
                {
                    //==========================получение сообщения============================

                    //объект, для формирования строк
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    //до тех пор, пока в потоке есть данные
                    do
                    {
                        //из потока считываются 64 байта и записываются в data начиная с 0
                        bytes = stream.Read(data, 0, data.Length);

                        //из считанных данных формируется строка
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    //преобразование сообщения
                    string message = builder.ToString();

                    //вывод сообщения в лог сервера
                    Dispatcher.BeginInvoke(new Action(() => log.Items.Add(message)));

                    //==========================отправка сообщения=============================

                    //преобразование сообщения в набор байт
                    data = Encoding.Unicode.GetBytes(message);

                    //отправка сообщения обратно клиенту
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex) //если возникла ошибка, вывести сообщение об ошибке
            {
                Dispatcher.BeginInvoke(new Action(() => log.Items.Add(ex.Message)));
            }
            finally //после выхода из бесконечного цикла
            {
                //освобождение ресурсов при завершении сеанса
                if (stream != null)
                    stream.Close();

                if (client != null)
                    client.Close();
            }
        }
    } 
}
