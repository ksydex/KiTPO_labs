using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using KiTPO.Extensions;
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
        public static double ScaleValue = 114; // 1 on coord. is 114px
        public string FilePath = "out" + DateTime.UtcNow.Millisecond + ".txt";

        public ObservableCollection<string> OutputList { get; set; } = new();

        public MainWindow()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            InitializeComponent();
            CenterCoordinate = MainImage.Width / 2;
            OutputListView.ItemsSource = OutputList;
            SetInputValues(0, 0);
            // TODO
            // 1. Обработать длинные числа +
            // 2. Ввод с помощью случайных чисел +
            // 3. Указать в логе откуда получены данные +
        }


        private async void WriteToOutput(string v)
        {
            OutputList.Add(v);
            OutputListView.SelectedIndex = OutputList.Count - 1;
            OutputListView.ScrollIntoView(OutputListView.SelectedItem);

            await using StreamWriter file = new(FilePath, append: true);
            await file.WriteLineAsync(v);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(MainImage);
            WriteToOutput("Точка вводится вручную нажатием по графику");
            ProcessPoint(pos.X, pos.Y);
        }

        private void ResetCanvas()
        {
            var toRemove = new List<UIElement>();
            foreach (UIElement el in Canvas.Children)
                if (el is Ellipse)
                    toRemove.Add(el);
            foreach (var el in toRemove)
                Canvas.Children.Remove(el);
        }

        private void ProcessPoint(double x, double y, bool append = false)
        {
            if (!append) ResetCanvas();

            var newEllipse = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Red),
                Width = 5,
                Height = 5
            };
            Canvas.Children.Add(newEllipse);
            Canvas.SetLeft(newEllipse, x);
            Canvas.SetTop(newEllipse, y);
            var (position, xFlatten, yFlatten) =
                CoordinatesProcessing.GetPointPosition(x, y, CenterCoordinate, ScaleValue);

            if (!xFlatten.IsInRange() || !yFlatten.IsInRange()) WriteToOutput("Обнаружен выход за пределы допустимого числового диапазона");
            else WriteToOutput(CoordinatesProcessing.GenerateMessage(position, xFlatten, yFlatten));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var x = XInput.Value;
            var y = YInput.Value;

            if (x != null && y != null)
            {
                (x, y) = CoordinatesProcessing.UnFlattenCoordinates(x.Value, y.Value, CenterCoordinate, ScaleValue);
                WriteToOutput("Точка вводится вручную через поля ввода");
                ProcessPoint(x.Value, y.Value);
            }
            else WriteToOutput("Данные не были введены");
            SetInputValues(0, 0);
        }

        private async void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { DefaultExt = ".txt", Filter = "Text Files (*.txt)|*.txt" };

            // Display OpenFileDialog by calling ShowDialog method 
            var result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                ResetCanvas();
                var filename = dlg.FileName;
                var (err, list) = await FileProcessing.GetPointsFromFile(filename);
                WriteToOutput(FileProcessing.GenerateMessage((err, list)));
                if (list != null)
                {
                    foreach (var point in list)
                    {
                        var (x, y) =
                            CoordinatesProcessing.UnFlattenCoordinates(point.Item1, point.Item2, CenterCoordinate,
                                ScaleValue);
                        ProcessPoint(x, y, true);
                    }
                }
            }
        }

        private void SetInputValues(double x, double y)
        {
            XInput.Text = x.ToString(CultureInfo.InvariantCulture);
            YInput.Text = y.ToString(CultureInfo.InvariantCulture);
        }

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
            => SetInputValues(NumberExtensions.RandomInRange(NumberExtensions.Min, NumberExtensions.Max),
                NumberExtensions.RandomInRange(NumberExtensions.Min, NumberExtensions.Max));
    }
}