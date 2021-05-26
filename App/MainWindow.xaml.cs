using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using KiTPO.Helpers;

namespace KiTPO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double CenterCoordinate { get; set; }
        public double ScaleValue = 114; // 1 on coord. is 112px
        public const double Accuracy = 0.02;

        public ObservableCollection<string> OutputList { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            CenterCoordinate = MainImage.Width / 2;
            OutputListView.ItemsSource = OutputList;
        }


        private void PushToOutput(string v)
        {
            OutputList.Add(v);
        }

        private void MainImage_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(MainImage);

            Point.Fill = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(Point, pos.X);
            Canvas.SetTop(Point, pos.Y);

            PushToOutput(CoordinateHelpers.GetPointPosition(pos.X, pos.Y, CenterCoordinate, ScaleValue).ToString());
        }
    }

    public enum PointPosition
    {
        Inside = 0,
        OnTheBorder = 1,
        Outside = 2
    }
}