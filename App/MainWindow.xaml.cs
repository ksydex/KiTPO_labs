using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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
using Microsoft.Win32;

namespace KiTPO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double CenterCoordinate { get; set; }
        public double ScaleValue = 114; // 1 on coord. is 114px

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
            OutputListView.SelectedIndex = OutputList.Count - 1;
            OutputListView.ScrollIntoView(OutputListView.SelectedItem);
        }

        private void MainImage_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(MainImage);
            ProcessPoint(pos.X, pos.Y);
        }

        private void ProcessPoint(double x, double y, bool append = false)
        {
            if (!append)
            {
                var toRemove = new List<UIElement>();
                foreach (UIElement el in Canvas.Children)
                    if(el is Ellipse) toRemove.Add(el);
                foreach (var el in toRemove)
                    Canvas.Children.Remove(el);
            }
            
            var newEllipse = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Red),
                Width = 5,
                Height = 5
            };
            Canvas.Children.Add(newEllipse);
            Canvas.SetLeft(newEllipse, x);
            Canvas.SetTop(newEllipse, y);
            var (position, xFlatten, yFlatten) = CoordinatesProcessing.GetPointPosition(x, y, CenterCoordinate, ScaleValue);
            PushToOutput(CoordinatesProcessing.GenerateMessage(position, xFlatten, yFlatten));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var x = XInput.Value;
            var y = YInput.Value;
            if (x != null && y != null)
            {
                (x, y) = CoordinatesProcessing.UnFlattenCoordinates(x.Value, y.Value, CenterCoordinate, ScaleValue);
                ProcessPoint(x.Value, y.Value);
                XInput.Value = null;
                YInput.Value = null;
            }
            else PushToOutput("Ошибка ввода данных");
        }

        private async void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { DefaultExt = ".txt", Filter = "Text Files (*.txt)|*.txt" };

            // Display OpenFileDialog by calling ShowDialog method 
            var result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                var filename = dlg.FileName;
                await FileProcessing.GetPointsFromFile(filename);
            }
        }
    }
}