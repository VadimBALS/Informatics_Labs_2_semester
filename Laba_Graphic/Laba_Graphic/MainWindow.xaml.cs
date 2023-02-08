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
using System.Windows.Threading;

/*
Необходимо разработать и реализовать примеры приведенные в лабораторных работах.

    Требуется реализовать “часы”: используя фигуры “эллипс” и “линия”, а также, 
    объект DispatcherTimer, реализовать двумерные часы со стрелками.
*/
namespace Laba_Graphic
{
    public partial class MainWindow : Window
    {
        Rectangle chel = new Rectangle(); //объект для рисования кадра анимации
                                          //ширина и высота прямоугольника, совпадает с размерами кадра

        // стрелка часов 
        Line sec = new Line();
        // стрелка часов
        Line min = new Line();
        // стрелка часов
        Line hour = new Line();

        // угол секунд 
        int secang = 0;
        // угол минут
        int minang = 0;
        // угол часов
        int hang = 0;

        // таймекр для анимации чела
        DispatcherTimer dtср = new DispatcherTimer();

        // таймер для анимации стрелки секунд
        DispatcherTimer dts = new DispatcherTimer();
        // таймер для анимации стрелки минут
        DispatcherTimer dtm = new DispatcherTimer();
        // таймер для анимации стрелки часов
        DispatcherTimer dth = new DispatcherTimer();

        // кол-во фаз внимации в 1 строке 
        int frameCount = 8;
        // номер текущего кадра 
        int currentFrame = 0;
        // ширина кадра  
        int frameW = 128;
        // высота кадра 
        int frameH = 128;
        // текущая строка  
        int currentRow = 4;

        // координаты 
        double posX = 0;
        double posY = 0;

        // кнопки 
        bool k = false;
        bool j = false;
        bool i = false;
        bool l = true;

        public MainWindow()
        {
            InitializeComponent();

            dtср.Interval = new TimeSpan(0, 0, 0, 0, 125);
            dtср.Tick += Dtch_Tick;

            dts.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dts.Tick += Dts_Tick;

            dtm.Interval = new TimeSpan(0, 0, 0, 2);
            dtm.Tick += Dtm_Tick;

            dth.Interval = new TimeSpan(0, 0, 0, 5);
            dth.Tick += Dth_Tick;
        }

        private void Dts_Tick(object sender, EventArgs e)
        {
            secang += 6;
            sec.RenderTransform = new RotateTransform(secang%360, 275, 195);
        }
        private void Dtm_Tick(object sender, EventArgs e)
        {
            minang += 6;
            min.RenderTransform = new RotateTransform(minang % 360, 275, 195);
        }
        private void Dth_Tick(object sender, EventArgs e)
        {
            hang += 6;
            hour.RenderTransform = new RotateTransform(hang % 360, 275, 195);
        }
        
        private void Dtch_Tick(object sender, EventArgs e)
        {
            //определение номера текущего кадра (текущий кадр + 1 + общее число кадров) % общее число кадров
            currentFrame = (currentFrame + 1 + frameCount) % frameCount;
            
            //вычисление координат кадра - номер текущего кадра * ширина/высота одного кадра
            var frameLeft = currentFrame * frameW;
            var frameTop = currentRow * frameH;
            
            //изменение отображаемого участка
            (chel.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop +
            frameH);

            // выбор направления перемещения в зависимости от клавиши 
            if (l)
                posX += 5;
            if (j)
                posX -= 5;
            if (i)
                posY -= 5;
            if (k)
                posY += 5;

            // перемещение 
            chel.RenderTransform = new TranslateTransform(posX, posY);
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            cnv.Children.Clear();

            dtср.Stop();

            dts.Stop();
            dtm.Stop();
            dth.Stop();
        }
        private void dr_line_Click(object sender, RoutedEventArgs e)
        {
            //создание объекта линия
            Line myLine = new Line();

            //установка цвета линии
            myLine.Stroke = System.Windows.Media.Brushes.Black;

            //координаты начала линии
            myLine.X1 = 1;
            myLine.Y1 = 1;

            //координаты конца линии
            myLine.X2 = 50;
            myLine.Y2 = 50;

            //параметры выравнивания в сцене
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;

            //толщина линии
            myLine.StrokeThickness = 2;

            //отступы
            myLine.Margin = new Thickness(50, 50, 0, 0);

            //добавление линии в сцену
            cnv.Children.Add(myLine);
        }
        private void dr_circle_Click(object sender, RoutedEventArgs e)
        {
            //создание объекта овал
            Ellipse myEllipse = new Ellipse();

            //создание объекта кисть
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            //установка цвета в виде сочетания компонент ARGB (alpha, red, green, blue)
            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);

            //установка объекта кисти в параметр заливки объекта овал
            myEllipse.Fill = mySolidColorBrush;

            //толщина и цвет обводки
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.Black;

            //размеры овала
            myEllipse.Width = 100;
            myEllipse.Height = 100;

            //позиция овала
            myEllipse.Margin = new Thickness(50, 50, 0, 0);

            //добавление овала в сцену
            cnv.Children.Add(myEllipse);

        }
        private void dr_rectangle_Click(object sender, RoutedEventArgs e)
        {
            //создание объекта прямоугольник
            Rectangle myRect = new Rectangle();

            //установка цвета линии обводки и цвета заливки при помощи коллекции кистей
            myRect.Stroke = Brushes.Black;
            myRect.Fill = Brushes.SkyBlue;

            //параметры выравнивания
            myRect.HorizontalAlignment = HorizontalAlignment.Left;
            myRect.VerticalAlignment = VerticalAlignment.Center;

            //размеры прямоугольника
            myRect.Height = 50;
            myRect.Width = 50;

            //отступы
            myRect.Margin = new Thickness(50, 50, 0, 0);

            //добавление объекта в сцену
            cnv.Children.Add(myRect);
        }
        private void dr_polygon_Click(object sender, RoutedEventArgs e)
        {
            //создание объекта многоугольник
            Polygon myPolygon = new Polygon();

            //установка цвета обводки, цвета заливки и толщины обводки
            myPolygon.Stroke = Brushes.Black;
            myPolygon.Fill = Brushes.LightSeaGreen;
            myPolygon.StrokeThickness = 2;

            //позиционирование объекта
            myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
            myPolygon.VerticalAlignment = VerticalAlignment.Center;

            //создание точек многоугольника
            Point Point1 = new Point(0, 0);
            Point Point2 = new Point(100, 0);
            Point Point3 = new Point(100, 50);
            Point Point4 = new Point(50, 100);
            Point Point5 = new Point(0, 50);

            //создание и заполнение коллекции точек
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(Point1);
            myPointCollection.Add(Point2);
            myPointCollection.Add(Point3);
            myPointCollection.Add(Point4);
            myPointCollection.Add(Point5);

            //установка коллекции точек в объект многоугольник
            myPolygon.Points = myPointCollection;

            //отступы
            myPolygon.Margin = new Thickness(50, 50, 0, 0);

            //добавление многоугольника в сцену
            cnv.Children.Add(myPolygon);

        }
        private void dr_heart_Click(object sender, RoutedEventArgs e)
        {
            //создание объекта путь и установка параметров его отрисовки
            Path path = new Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1;

            //создание двух сегментов пути при помощи кривых Безье
            //параметры - (первая контрольная точка, вторая контрольная точка, конец кривой)
            BezierSegment bezierCurve1 = new BezierSegment(new Point(0, 0), new Point(0, 50), new Point(50, 90),
            true);
            BezierSegment bezierCurve2 = new BezierSegment(new Point(100, 50), new Point(100, 0), new Point(50,
            30), true);


            //создание коллекции сегментов и добавление к ней кривых
            PathSegmentCollection psc = new PathSegmentCollection();
            psc.Add(bezierCurve1);
            psc.Add(bezierCurve2);

            //создание объекта фигуры и установка начальной точки пути
            PathFigure pf = new PathFigure();
            pf.Segments = psc;
            pf.StartPoint = new Point(50, 30);

            //создание коллекции фигур
            PathFigureCollection pfc = new PathFigureCollection();
            pfc.Add(pf);

            //создание геометрии пути
            PathGeometry pg = new PathGeometry();
            pg.Figures = pfc;

            //создание набора геометрий
            GeometryGroup pathGeometryGroup = new GeometryGroup();
            pathGeometryGroup.Children.Add(pg);

            path.Data = pathGeometryGroup;

            //отступы
            path.Margin = new Thickness(50, 50, 0, 0);

            //добавление объекта путь в сцену
            cnv.Children.Add(path);

        }
        private void dr_meme_Click(object sender, RoutedEventArgs e)
        {
            //создание объекта овал
            Ellipse myEllipse = new Ellipse();

            //кисть для заполнения прямоугольника изображением
            ImageBrush ib = new ImageBrush();

            //позиция изображения будет указана как координаты левого верхнего угла
            //изображение будет растянуто по размерам прямоугольника, описанного вокруг фигуры
            ib.AlignmentX = AlignmentX.Left;
            ib.AlignmentY = AlignmentY.Top;

            //загрузка изображения и назначение кисти
            ib.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/pic/gch.png", UriKind.Absolute));
            myEllipse.Fill = ib;

            //толщина и цвет обводки
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.Black;

            //размеры овала
            myEllipse.Width = 300;
            myEllipse.Height = 300;

            //позиция овала
            myEllipse.Margin = new Thickness(50, 50, 0, 0);

            //добавление овала в сцену
            cnv.Children.Add(myEllipse);
        }

        private void BOOM_Click(object sender, RoutedEventArgs e)
        {
            chel.Height = 128;
            chel.Width = 128;

            //кисть для заполнения прямоугольника фрагментом изображения
            ImageBrush ib = new ImageBrush();

            //настройки, позиция изображения будет указана как координаты левого верхнего угла
            //изображение будет выведено без растяжения/сжатия
            ib.AlignmentX = AlignmentX.Left;
            ib.AlignmentY = AlignmentY.Top;
            ib.Stretch = Stretch.None;

            //участок изображения который будет нарисован
            ib.Viewbox = new Rect(0, 0, 64, 64);
            ib.ViewboxUnits = BrushMappingMode.Absolute;

            //загрузка изображения и назначение кисти
            ib.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/pic/peop.png",
            UriKind.Absolute));
            chel.Fill = ib;

            // увеличение чела 
            chel.RenderTransform = new ScaleTransform(1.1, 1.1);

            //добавление прямоугольника в сцену
            cnv.Children.Add(chel);

            // запуск таймера 
            dtср.Start();
        }
        private void way_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.I) // /\
            {
                i = true; k = false; l = false; j = false;
                currentRow = 2;
            }
            if (e.Key == Key.K) // \/
            {
                k = true; i = false; l = false; j = false;
                currentRow = 6;
            }
            if (e.Key == Key.L) // >
            {
                l = true; k = false; i = false;  j = false;
                currentRow = 4;
            }
            if (e.Key == Key.J) // <
            {
                j = true; k = false; i = false; l = false; 
                currentRow = 0;
            }

        }

        private void watch_Click(object sender, RoutedEventArgs e)
        {
            //создание объекта овал
            Ellipse myEllipse = new Ellipse();

            //создание объекта кисть
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            //установка цвета в виде сочетания компонент ARGB (alpha, red, green, blue)
            mySolidColorBrush.Color = Color.FromArgb(255, 78, 205, 237);

            //установка объекта кисти в параметр заливки объекта овал
            myEllipse.Fill = mySolidColorBrush;
            
            //установка цвета линии
            sec.Stroke = System.Windows.Media.Brushes.Black;
            min.Stroke = System.Windows.Media.Brushes.Black;
            hour.Stroke = System.Windows.Media.Brushes.Black;

            //толщина и цвет обводки
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.Black;

            //размеры овала
            myEllipse.Width = 350;
            myEllipse.Height = 350;

            //позиция овала
            myEllipse.Margin = new Thickness(100, 20, 0, 0);

            //добавление овала в сцену
            cnv.Children.Add(myEllipse);

            //координаты начала линии
            sec.X1 = 275;   min.X1 = 275;   hour.X1 = 275;
            sec.Y1 = 195;   min.Y1 = 195;   hour.Y1 = 195;

            //координаты конца линии
            sec.X2 = 275;   min.X2 = 275; hour.X2 = 275;
            sec.Y2 = 35;    min.Y2 = 65; hour.Y2 = 95;

            //параметры выравнивания в сцене
            sec.HorizontalAlignment = HorizontalAlignment.Left;
            sec.VerticalAlignment = VerticalAlignment.Center;
            min.HorizontalAlignment = HorizontalAlignment.Left;
            min.VerticalAlignment = VerticalAlignment.Center;
            hour.HorizontalAlignment = HorizontalAlignment.Left;
            hour.VerticalAlignment = VerticalAlignment.Center;
            
            //толщина линии
            sec.StrokeThickness = 2;
            min.StrokeThickness = 3;
            hour.StrokeThickness = 5;

            //добавление линий в сцену
            cnv.Children.Add(sec);
            cnv.Children.Add(min);
            cnv.Children.Add(hour);

            dts.Start();
            dtm.Start();
            dth.Start();
        }
    }
}
