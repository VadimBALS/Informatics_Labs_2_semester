﻿using System;
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

//подключение библиотек для работы с сетью и потоками
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Linq;

namespace Client
{
    public partial class MainWindow : Window
    {

        //номер порта для обмена сообщениями
        int port = 8888;
        //ip адрес сервера
        string address = "127.0.0.1";
        //объявление TCP клиента
        TcpClient client = null;
        //объявление канала соединения с сервером
        NetworkStream stream = null;
        //имя пользователя
        string username = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void connect_to_server_Click(object sender, RoutedEventArgs e)
        {
            //получение имени пользователя
            username = uName.Text;
            try //если возникнет ошибка - переход в catch
            {
                //создание клиента
                client = new TcpClient(address, port);
                //получение канала для обмена сообщениями
                stream = client.GetStream();

                //получение сообщения
                string message = username;

                //преобразование имени в массив байтов
                byte[] data = Encoding.Unicode.GetBytes(message);

                //отправка сообщения
                stream.Write(data, 0, data.Length);

                //создание нового потока для ожидания сообщения от сервера
                Thread listenThread = new Thread(() => listen());
                listenThread.Start();
            }
            catch (Exception ex)
            {
                log.Items.Add(ex.Message); 
            }

        }

        //функция ожидания сообщений от сервера
        void listen()
        {
            try //в случае возникновения ошибки - переход к catch
            {
                //цикл ожидания сообщениями
                while (true)
                {
                    //буфер для получаемых данных
                    byte[] data = new byte[64];
                    
                    //объект для построения смтрок
                    StringBuilder builder = new StringBuilder();
                    
                    int bytes = 0;
                    
                     //до тех пор, пока есть данные в потоке
                    do
                    {
                        //получение 64 байт
                        bytes = stream.Read(data, 0, data.Length);
                        
                        //формирование строки
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    
                    //получить строку
                    string message = builder.ToString();
                    
                    //вывод сообщения в лог клиента
                    Dispatcher.BeginInvoke(new Action(() => log.Items.Add("Сервер: " + message)));
                }
            }
            catch (Exception ex)
            {
                //вывести сообщение об ошибке
                Dispatcher.BeginInvoke(new Action(() => log.Items.Add(ex.Message)));
            }
            finally
            {
                //закрыть канал связи и завершить работу клиента
                stream.Close();
                client.Close();
            }
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            //получение сообщения
            string message = msg.Text;

            // очистка поля сообщения 
            msg.Text = "";
            
            //добавление имени пользователя к сообщению
            message = String.Format("{0}: {1}", username, message);
            
            //преобразование сообщение в массив байтов 
            byte[] data = Encoding.Unicode.GetBytes(message);
            
            //отправка сообщения
            stream.Write(data, 0, data.Length);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            stream.Close();
            client.Close();
        }
    }
}
