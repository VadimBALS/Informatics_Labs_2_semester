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
    1) Ввод данных о студентах: уникальный номер, фио, оценка по физике, оценка по    
математике.
    2) Добавление данных в таблицу базы данных SQLite через интерфейс приложения.     
    3) Чтение данных из базы данных SQLite и отображение их на форме приложения.      YES
    4) Редактирование данных в базе данных SQLite через интерфейс приложения.         
    5) Удаление данных из таблиц                                                      

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
        // подключение к БД
        SQLiteConnection m_dbConnection;

        public MainWindow()
        {
            InitializeComponent();

            /*
            SQLiteConnection.CreateFile("C:\\Users\\user\\Desktop\\HEI\\Semestr_2\\Informatics\\C#\\Laba_DB\\test.db");

            // открытие окна для поиска файла БД
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            // имя базы данных
            string db_name = dlg.FileName;

            // подключение к БД
            m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");

            //открытие соединения с базой данных
            m_dbConnection.Open();


            string sql = "CREATE TABLE students (uid INTEGER NOT NULL, fio TEXT NOT NULL, PRIMARY KEY (\"uid\"))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "CREATE TABLE grades (uid INTEGER NOT NULL, math INTEGER, phys INTEGER)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();


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

        // структура для записи данных в БД
        struct SField
        {
            public string fio { get; set; }
            public int math { get; set; }
            public int phys { get; set; }
        }

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
            // формирование запроса для вывода данных из БД (ФИО, оценка матеша, оценка физика)
            string sql = "select students.fio, grades.math, grades.phys from students, grades where students.uid=grades.uid";
            
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

                // запись данных в DataGrid
                data.Items.Add(f);
            }
        }

        private void Button_Click_add_student(object sender, RoutedEventArgs e)
        {
            Wnd_1 window = new Wnd_1();

            if (window.ShowDialog() == true)
            {
                int count_students = 0;
                
                string sql = "SELECT* FROM students, grades WHERE students.uid = grades.uid";
                
                // задаётся команда 
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

                // извлекается 
                SQLiteDataReader reader = command.ExecuteReader();
                
                // подсчёт скока всего студентов в таблице
                while (reader.Read())
                    count_students++;

                // кол-во студентов 
                lb.Content = count_students++;

                // формирование запроса для вывода данных из БД (ФИО, оценка матеша, оценка физика)
                sql = $"INSERT INTO `grades` (`uid`, `math`, `phys`) VALUES ( {count_students}, {int.Parse(window.g_math.Text)}, {int.Parse(window.g_phys.Text)});" +
                      $"INSERT INTO `students` (`uid`, `fio`) VALUES ( {count_students}, \"{window.name.Text}\");";

                // задаётся команда 
                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                // кол-во студентов 
                lb.Content = count_students+1;
            }
        }

        private void Button_Click_delete_student(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //закрытие соединения с базой данных
            if (m_dbConnection != null)
                m_dbConnection.Close();
        }
    }
}
