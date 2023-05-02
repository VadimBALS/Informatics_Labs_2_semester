using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using System.Data.SQLite;
using System.Windows.Threading;
using System.Collections;

namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        // картинки 
        BitmapImage mine  = new BitmapImage(new Uri(@"pack://application:,,,/img/mine.png", UriKind.Absolute));
        BitmapImage egg   = new BitmapImage(new Uri(@"pack://application:,,,/img/egg_pixelart.png", UriKind.Absolute));
        BitmapImage egg_0 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_0.png", UriKind.Absolute));
        BitmapImage egg_1 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_1.png", UriKind.Absolute));
        BitmapImage egg_2 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_2.png", UriKind.Absolute));
        BitmapImage egg_3 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_3.png", UriKind.Absolute));
        BitmapImage egg_4 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_4.png", UriKind.Absolute));
        BitmapImage egg_5 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_5.png", UriKind.Absolute));
        BitmapImage egg_6 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_6.png", UriKind.Absolute));
        BitmapImage egg_7 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_7.png", UriKind.Absolute));
        BitmapImage egg_8 = new BitmapImage(new Uri(@"pack://application:,,,/img/aliens_egg_8.png", UriKind.Absolute));

        //звук смерти 
        SoundPlayer death = new SoundPlayer();

        // музыка 
        MediaPlayer player = new MediaPlayer();

        // размер кнопки 
        int size_btn = 50;

        // статусас
        bool fail = false;

        // кол-во мин
        int count_mines;
        
        // размер поля
        int size_N;
        
        // осталось клеток
        int ost;

        // экземпляр класса 
        FieldGen gen;

        // переменная для подключения к БД
        SQLiteConnection m_dbConnection;

        // путь к папке
        string path_to_folder_prj = "C:\\Users\\user\\Desktop\\HEI\\Semestr_2\\Informatics\\C#"; 


        // создание переменной Таймер
        System.Windows.Threading.DispatcherTimer Timer;
        DateTime start_time = new DateTime();
        DateTime playing_time = new DateTime();

        public MainWindow()
        {
            InitializeComponent();

            // блокировка кнопки 
            Start_Cl.IsEnabled = false;

            // добавление уровни сложностей 
            dif_var.Items.Add("Baby");
            dif_var.Items.Add("Boy");
            dif_var.Items.Add("Man");

            //// выбор медиа файла, например, в формате .mp3
            //OpenFileDialog dlg = new OpenFileDialog();
            //dlg.ShowDialog();

            //// загрузка выбранного файла
            //player.Open(new Uri(dlg.FileName, UriKind.Relative));
            // path to folder with project

            player.Open(new Uri(path_to_folder_prj + "\\Minesweeper\\Minesweeper\\music1\\alien_isolation.mp3"));
            
            // установка громкости
            player.Volume = volume.Value;
            // играй 
            player.Play();

            // подключение к БД    // path to folder with project
            m_dbConnection = new SQLiteConnection("Data Source=" + path_to_folder_prj +  "\\Minesweeper\\DB.db" + ";Version=3;");

            //открытие соединения с базой данных
            m_dbConnection.Open();


            //########### TIMER ###########
            //инициализация переменной таймер
            Timer = new System.Windows.Threading.DispatcherTimer();

            //назначение обработчика события Тик
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);

            //установка интервала между тиками
            //TimeSpan – переменная для хранения времени в формате часы/минуты/секунды
            Timer.Interval = new TimeSpan(0, 0, 1);


            show_data();
            /*
            // ################### CREATE DB FILE ################### 

            SQLiteConnection.CreateFile("D:...\...\Informatics\C#\Minesweeper\DB.bd");

            // открытие окна для поиска файла БД
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            // имя базы данных
            string db_name = dlg.FileName;

            // подключение к БД
            m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");

            //открытие соединения с базой данных
            m_dbConnection.Open();

            // ################### CREATE TABLE ################### 

            string sql = "CREATE TABLE players (uid INTEGER NOT NULL, nickname TEXT NOT NULL, time TEXT NOT NULL, PRIMARY KEY (\"uid\"))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();


            // ################### INSERT 2 PLAYERS ################### 
            
            sql = $"INSERT INTO `players` (`uid`, `nickname`, `time`) VALUES (1, \"Student_1\", \"00:00:44\");" +
                  $"INSERT INTO `players` (`uid`, `nickname`, `time`) VALUES (2, \"Student_2\", \"00:01:50\");";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            */
        
        }


        //################################## TIMER ##################################
        
        //обработчик события Тик
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            playing_time = DateTime.Now;
            TimeSpan r = playing_time - start_time;

            time.Content = ((int)(r.TotalSeconds)).ToString();
        }

        //################################## DB ################################## 

        // структура для записи данных из таблицы
        struct SPlayer
        {
            public int uid { get; set; }
            public string nickname { get; set; }
            public string time { get; set; }
        }
        private void show_data()
        {
            // очистка дистбокса БД
            this.data_players.Items.Clear();

            // формирование запроса для вывода данных из БД (UID, ники, время)
            string sql = "select players.uid, players.nickname, players.time from players";

            // задаётся команда 
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

            // извлекается 
            SQLiteDataReader reader = command.ExecuteReader();

            // результат работы запроса = reader (спец переменная с инфой из БД)
            // заполнение DataGrid данными
            while (reader.Read())
            {
                // создание новой структуры 
                SPlayer f = new SPlayer();

                // заполнение структуры
                f.uid = int.Parse(reader["uid"].ToString());
                f.nickname = reader["nickname"].ToString();
                f.time = reader["time"].ToString();

                // запись данных в DataGrid
                data_players.Items.Add(f);
            }
        }
        private void add_winner(string nick, string time)
        {
            // буфер для последнего UID для образования нового 
            int last_uid = 0;

            // формирование запроса для вывода данных из БД
            string sql = "select players.uid from players";

            // задаётся команда 
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

            // извлекается 
            SQLiteDataReader reader = command.ExecuteReader();

            // результат работы запроса = reader (спец переменная с инфой из БД)
            // находим последний UID для образования нового 
            while (reader.Read())
                last_uid = int.Parse(reader["uid"].ToString());

            // добавление новенького
            last_uid++;

            sql = $"INSERT INTO `players` (`uid`, `nickname`, `time`) VALUES ( {last_uid}, \"{nick}\", \"{time}\");";

            // отправка запроса  
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        //################################## GAME ################################## 

        private void Start_Cl_Click(object sender, RoutedEventArgs e)
        {
            // старут таймер и запись времени начала
            start_time = DateTime.Now;
            Timer.Start();

            // установка статуса 
            fail = false;

            // установка отрицательного доступа к кнопке
            Start_Cl.IsEnabled = false;

            // проверка сложности и вывод кол-ва сгенеренных мин, установка звука смерти,
            // определение кол-ва мин на поле
            if (dif_var.SelectedIndex != -1)
            {
                if (V.Text != "" && M.Text != "")
                {
                    int v = int.Parse(V.Text);
                    int m = int.Parse(M.Text);

                    gen = new FieldGen(dif_var.SelectedIndex, v, m);
                    gen.generate();

                    if (dif_var.SelectedIndex == 0)
                    {
                        count_mines = (int)(v * m * 20 / 100);
                        ost = v*m;
                        death.Stream = Properties.Resources.facehugger_baby;
                    }
                    else if (dif_var.SelectedIndex == 1)
                    {
                        count_mines = (int)(v * m * 25 / 100);
                        ost = v * m;
                        death.Stream = Properties.Resources.facehugger_boy;
                    }
                    else if (dif_var.SelectedIndex == 2)
                    {
                        count_mines = (int)(v * m * 30 / 100);
                        ost = v * m;
                        death.Stream = Properties.Resources.facehugger_man;
                    }

                    // выводим скока мин сгенерилось 
                    RealCount.Content = $"Generated mines: {count_mines}";

                    // очистка сетки перед новой генерацией
                    ugr.Children.Clear();

                    // кол-во строк и столбцов в сетке 
                    ugr.Rows = v;
                    ugr.Columns = m;

                    // размеры сетки = число ячеек * (размер кнопки + толщина границы)
                    ugr.Height = v * (size_btn + 4);
                    ugr.Width = m * (size_btn + 4);
                    

                    // толщина границ сетки 
                    ugr.Margin = new Thickness(5, 5, 5, 5);

                    // создание кнопок 
                    for (int i = 0; i < v * m; i++)
                    {
                        // создание кнопки 
                        Button btn = new Button();

                        // запись номера кнопки 
                        btn.Tag = i;

                        // установка размеров 
                        btn.Width = size_btn;
                        btn.Height = size_btn;

                        // добавление события 
                        btn.Click += Btn_Click;

                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        btn.Content = stackPnl;

                        // добавление в сетку 
                        ugr.Children.Add(btn);
                    }
                }
                else
                {
                    gen = new FieldGen(dif_var.SelectedIndex);
                    gen.generate();

                    if (dif_var.SelectedIndex == 0)
                    {
                        RealCount.Content = "Generated mines: 3";
                        count_mines = 3;
                        size_N = 4;
                        ost = 16;
                        death.Stream = Properties.Resources.facehugger_baby;
                    }
                    else if (dif_var.SelectedIndex == 1)
                    {
                        RealCount.Content = "Generated mines: 12";
                        count_mines = 12;
                        size_N = 7;
                        ost = 49;
                        death.Stream = Properties.Resources.facehugger_boy;
                    }
                    else if (dif_var.SelectedIndex == 2)
                    {
                        RealCount.Content = "Generated mines: 30";
                        count_mines = 30;
                        size_N = 11;
                        ost = 121;
                        death.Stream = Properties.Resources.facehugger_man;
                    }

                    // размер поля
                    int N = size_N;

                    // очистка сетки перед новой генерацией
                    ugr.Children.Clear();

                    // кол-во строк и столбцов в сетке 
                    ugr.Rows = N;
                    ugr.Columns = N;

                    // размеры сетки = число ячеек * (размер кнопки + толщина границы)
                    ugr.Width = N * (size_btn + 4);
                    ugr.Height = N * (size_btn + 4);

                    // толщина границ сетки 
                    ugr.Margin = new Thickness(5, 5, 5, 5);

                    // создание кнопок 
                    for (int i = 0; i < N * N; i++)
                    {
                        // создание кнопки 
                        Button btn = new Button();

                        // запись номера кнопки 
                        btn.Tag = i;

                        // установка размеров 
                        btn.Width = size_btn;
                        btn.Height = size_btn;

                        // добавление события 
                        btn.Click += Btn_Click;

                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        btn.Content = stackPnl;

                        // добавление в сетку 
                        ugr.Children.Add(btn);
                    }
                }

            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (V.Text != "" && M.Text != "") 
            {
                // получение кнопки 
                int i = (int)((Button)sender).Tag;

                int v = int.Parse(V.Text);
                int m = int.Parse(M.Text);

                if (gen.getCell(i % v, i / m) == 9)
                {
                    // обновление статуса
                    fail = true;

                    // остановка таймера и сброс 
                    Timer.Stop();

                    // сброс таймера
                    time.Content = "";

                    // обновление изображения 
                    // создание контейнера под картинку 
                    Image img = new Image();
                    // записть картинки 
                    img.Source = mine;
                    // создание компонента для отображения 
                    StackPanel stackPnl = new StackPanel();
                    // установка толщины границ компонента 
                    stackPnl.Margin = new Thickness(0);
                    // добавление контейнера с картинкой в компонент 
                    stackPnl.Children.Add(img);
                    // запись компонента в кнопку 
                    ((Button)sender).Content = stackPnl;

                    //звук смерти 
                    death.Play();

                    //уведомление о инкубаторстве
                    MessageBox.Show("  !!!NOW YOU INCUBATOR!!! \n For retry press Start");

                    //открытие доступа к кнопке
                    Start_Cl.IsEnabled = true;
                }
                else
                {
                    if (gen.getCell(i % v, i / m) == 0)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_0;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;


                    }
                    else if (gen.getCell(i % v, i / m) == 1)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_1;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % v, i / m) == 2)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_2;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % v, i / m) == 3)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_3;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % v, i / m) == 4)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_4;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % v, i / m) == 5)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_5;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % v, i / m) == 6)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_6;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % v, i / m) == 7)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_7;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % v, i / m) == 8)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_8;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                }
            }
            else
            {
                int N = size_N;
                // получение кнопки 
                int i = (int)((Button)sender).Tag;

                if (gen.getCell(i % N, i / N) == 9)
                {
                    // обновление статуса
                    fail = true;

                    // остановка таймера и сброс 
                    Timer.Stop();

                    // сброс таймера
                    time.Content = "";

                    // обновление изображения 
                    // создание контейнера под картинку 
                    Image img = new Image();
                        // записть картинки 
                        img.Source = mine;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;

                    //звук смерти 
                    death.Play();

                    //уведомление о инкубаторстве
                    MessageBox.Show("  !!!NOW YOU INCUBATOR!!! \n For retry press Start");

                    //открытие доступа к кнопке
                    Start_Cl.IsEnabled = true;
                }
                else
                {
                    if (gen.getCell(i % N, i / N) == 0)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_0;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;


                    }
                    else if (gen.getCell(i % N, i / N) == 1)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_1;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % N, i / N) == 2)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_2;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % N, i / N) == 3)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_3;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % N, i / N) == 4)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_4;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % N, i / N) == 5)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_5;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % N, i / N) == 6)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_6;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % N, i / N) == 7)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_7;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                    else if (gen.getCell(i % N, i / N) == 8)
                    {
                        // создание контейнера под картинку 
                        Image img = new Image();
                        // записть картинки 
                        img.Source = egg_8;
                        // создание компонента для отображения 
                        StackPanel stackPnl = new StackPanel();
                        // установка толщины границ компонента 
                        stackPnl.Margin = new Thickness(0);
                        // добавление контейнера с картинкой в компонент 
                        stackPnl.Children.Add(img);
                        // запись компонента в кнопку 
                        ((Button)sender).Content = stackPnl;
                    }
                }
            }

            // минус клетка 
            ost -= 1;
        
            if (!fail && ost == count_mines)
            {
                // уведомление о победе
                MessageBox.Show("  !!!YOU WIN!!! \n For retry press Start");
                
                // остановка таймера и сброс 
                Timer.Stop();

                // добавление победителя в базу 
                add_winner(nicknm.Text, time.Content.ToString());

                // сброс таймера
                time.Content = "";

                //открытие доступа к кнопке
                Start_Cl.IsEnabled = true;

                // показываем зачисление победителя
                show_data();
            }

        }

        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = volume.Value;
        }

        private void dif_var_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Start_Cl.IsEnabled = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //закрытие соединения с базой данных перед закрытием прилодения
            if (m_dbConnection != null)
                m_dbConnection.Close();
        }
    }
}
