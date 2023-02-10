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
using System.Data.SQLite;
using Microsoft.Win32;



/*
Разработать программное приложение WPF, имеющее графический интерфейс и реализующее
следующие функции:
    1) Ввод данных о студентах: уникальный номер, фио, оценка по физике, оценка по    YES
математике.
    2) Добавление данных в таблицу базы данных SQLite через интерфейс приложения.     YES
    3) Чтение данных из базы данных SQLite и отображение их на форме приложения.      YES
    4) Редактирование данных в базе данных SQLite через интерфейс приложения.         YES
    5) Удаление данных из таблиц                                                      YES

База данных должна содержать 2 таблицы, связанные через уникальный номер:
    1. Таблица, содержащая уникальный номер и фио.                                    YES
    2. Таблица, содержащая уникальный номер и оценки.                                 YES
*/

/*
        Гайд по создаю БД основы
 В БД браузере создать новую бд в папке с проектом с названием test
  Добавление таблицы students  
   1 поле: имя uid, тип int, не пустое, первичный ключ, автоинкремент, уникальное 
   2 поле: имя fio, тип text, не пустое  
    OK
  Добавление таблицы grades   
   1 поле: имя uid, тип int, не пустое, первичный ключ, автоинкремент, уникальное 
   2 поле: имя math, тип int
   3 поле: имя phys, тип int
    OK
  Добавить пару students через "добавление поллей"
   1 юзер Применить
   2 юзер Применить
    Записать изменения 
*/

namespace Laba_DB
{
    public partial class MainWindow : Window
    {
        // переменная для подключения к БД
        SQLiteConnection m_dbConnection;

        public MainWindow()
        {
            InitializeComponent();

            /*
            // ################### CREATE DB FILE ################### 
             
            SQLiteConnection.CreateFile(""D:\\MyDocs\\method\\intsys\\test.db"");

            // открытие окна для поиска файла БД
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            // имя базы данных
            string db_name = dlg.FileName;

            // подключение к БД
            m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");

            //открытие соединения с базой данных
            m_dbConnection.Open();

            // ################### CREATE 2 TABLES ################### 

            string sql = "CREATE TABLE students (uid INTEGER NOT NULL, fio TEXT NOT NULL, PRIMARY KEY (\"uid\"))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "CREATE TABLE grades (uid INTEGER NOT NULL, math INTEGER, phys INTEGER)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();


            // ################### INSERT 3 STUDENTS ################### 

            sql = $"INSERT INTO `grades` (`uid`, `math`, `phys`) VALUES (1, 3, 3);" +
                  $"INSERT INTO `students` (`uid`, `fio`) VALUES (1, \"Student 1\");";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = $"INSERT INTO `grades` (`uid`, `math`, `phys`) VALUES (2, 4, 4);" +
                  $"INSERT INTO `students` (`uid`, `fio`) VALUES (2, \"Student 2\");";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = $"INSERT INTO `grades` (`uid`, `math`, `phys`) VALUES (3, 5, 5);" +
                  $"INSERT INTO `students` (`uid`, `fio`) VALUES (3, \"Student 3\");";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            */
        }

        // структура для записи данных из двух таблиц
        struct SField
        {
            public string fio { get; set; }
            public int math { get; set; }
            public int phys { get; set; }
            public int uid { get; set; }
        }

        // структура для записи данных из таблицы stydents
        struct STuds
        {
            public string fio { get; set; }
            public int uid { get; set; }
        }

        // структура для записи данных из таблицы grades
        struct GRades
        {
            public int math { get; set; }
            public int phys { get; set; }
            public int uid { get; set; }
        }

        // отслеживание сколько всего студентов в БД и Уникальный Идентификационный Номер (UID)
        int number_of_students = 0;

        private void Button_Click_start_db_con(object sender, RoutedEventArgs e)
        {
            // открытие окна для поиска файла БД
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            // имя базы данных
            string db_name = dlg.FileName;

            // подключение к БД
            m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");

            //открытие соединения с базой данных
            m_dbConnection.Open();
        }

        private void Button_Click_show_data(object sender, RoutedEventArgs e)
        {
            // очистка дистбокса БД
            this.data.Items.Clear();

            // обнуление кол-вa студентов 
            number_of_students = 0;

            // формирование запроса для вывода данных из БД (ФИО, оценка матеша, оценка физика)
            string sql = "select students.uid, students.fio, grades.math, grades.phys from students, grades where students.uid=grades.uid";
            
            // задаётся команда 
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            
            // извлекается 
            SQLiteDataReader reader = command.ExecuteReader();

            // результат работы запроса = reader (спец переменная с инфой из БД)
            // заполнение DataGrid данными
            while (reader.Read())
            {
                // создание новой структуры 
                SField f = new SField();

                // заполнение структуры
                f.fio = reader["fio"].ToString();
                f.math = int.Parse(reader["math"].ToString());
                f.phys = int.Parse(reader["phys"].ToString());
                f.uid = int.Parse(reader["uid"].ToString());

                // запись данных в DataGrid
                data.Items.Add(f);

                number_of_students++;
            }

            // работа с табоицей students
            this.data_stud.Items.Clear();
            sql = "select students.uid, students.fio from students";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                STuds f = new STuds();

                f.uid = int.Parse(reader["uid"].ToString());
                f.fio = reader["fio"].ToString();
                
                data_stud.Items.Add(f);
            }

            // работа с таблицей grades
            this.data_grad.Items.Clear();
            sql = "select grades.uid, grades.math, grades.phys from grades";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                GRades f = new GRades();

                f.uid = int.Parse(reader["uid"].ToString());
                f.phys = int.Parse(reader["phys"].ToString());
                f.math = int.Parse(reader["math"].ToString());

                data_grad.Items.Add(f);
            }

            // вывод кол-ва студентов 
            lb.Content = "Number of students: " + number_of_students;
        }

        private void Button_Click_add_student(object sender, RoutedEventArgs e)
        {
            // буфер для последнего UID для образования нового 
            int last_uid = 0;

            // формирование запроса для вывода данных из БД
            string sql = "select students.uid from students";

            // задаётся команда 
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

            // извлекается 
            SQLiteDataReader reader = command.ExecuteReader();

            // результат работы запроса = reader (спец переменная с инфой из БД)
            // находим последний UID для образования нового 
            while (reader.Read())
                last_uid = int.Parse(reader["uid"].ToString());

            Wnd_1 window = new Wnd_1();

            if (window.ShowDialog() == true)
            {
                // добавление новенького
                last_uid++;

                // формирование запроса
                sql = $"INSERT INTO `grades` (`uid`, `math`, `phys`) VALUES ( {last_uid}, {int.Parse(window.g_math.Text)}, {int.Parse(window.g_phys.Text)});" +
                      $"INSERT INTO `students` (`uid`, `fio`) VALUES ( {last_uid}, \"{window.name.Text}\");";

                // отправка запроса  
                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
        }

        private void Button_Click_delete_student(object sender, RoutedEventArgs e)
        {
            // проверка на наличие выделенного поля в гриде 
            if (data.SelectedIndex > -1)
            {
                // создание экземпляра структуры и загрузка в неё данных из грида
                SField test = (SField)(data.SelectedItem);

                // создание запроса 
                string sql = $"DELETE FROM students WHERE uid = {test.uid};" +
                             $"DELETE FROM grades WHERE uid = {test.uid};";

                // отправка запроса 
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
        }

        private void edit_entry_Click(object sender, RoutedEventArgs e)
        {
            // проверка на наличие выделенного поля в гриде 
            if (data.SelectedIndex > -1)
            {
                // создание экземпляра структуры и загрузка в неё данных из грида
                SField test = (SField)(data.SelectedItem);

                Wnd_2 window = new Wnd_2(test.fio, test.math, test.phys);

                if (window.ShowDialog() == true)
                {
                    // создание запроса 
                    string sql = $"UPDATE students SET fio = \"{window.name.Text}\" WHERE uid = {test.uid};" +
                                 $"UPDATE grades SET math = {int.Parse(window.g_math.Text)}, phys = {int.Parse(window.g_phys.Text)} WHERE uid = {test.uid};";

                    // отправка запроса 
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //закрытие соединения с базой данных перед закрытием прилодения
            if (m_dbConnection != null)
                m_dbConnection.Close();
        }
    }
}
