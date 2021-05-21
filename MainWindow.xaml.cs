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

namespace KTiPO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly double CanvasHeight;
        public readonly double CanvasWidth;
        public readonly double XRoot = 100;
        public readonly double YRoot = 100;
        public static readonly double Step = 10;

        
        public MainWindow()
        {
            InitializeComponent();
            CanvasHeight = Graph.Height;
            CanvasWidth = Graph.Width;
            Draw();
        }

        public void Draw()
        {
            DrawCoordinatesSystem();

            // Make some data sets.
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };
            Random rand = new Random();
            for (int data_set = 0; data_set < 3; data_set++)
            {
                int last_y = rand.Next(0, (int) CanvasHeight);

                PointCollection points = new PointCollection();
                for (double x = 0; x <= 0; x += Step)
                {
                    last_y = rand.Next(last_y - 10, last_y + 10);
                    if (last_y < 0) last_y = 0;
                    if (last_y > CanvasHeight) last_y = (int) CanvasHeight;
                    points.Add(new Point(x, last_y));
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = brushes[data_set];
                polyline.Points = points;

                Graph.Children.Add(polyline);
            }
        }

        public void DrawCoordinatesSystem()
        {
            const double lineLength = 4;
            const double step = 5;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(0, YRoot), new Point(Graph.Width, YRoot)));
            for (double x = step;
                x <= Graph.Width - step;
                x += step)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, YRoot - lineLength / 2),
                    new Point(x, YRoot + lineLength / 2)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            Graph.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(XRoot, 0), new Point(XRoot, Graph.Height)));
            for (double y = step; y <= Graph.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(XRoot - lineLength / 2, y),
                    new Point(XRoot + lineLength / 2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Red;
            yaxis_path.Data = yaxis_geom;

            Graph.Children.Add(yaxis_path);            
        }
        
        private void Graph_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(Mouse.GetPosition(Graph).X);
        }
    }
}