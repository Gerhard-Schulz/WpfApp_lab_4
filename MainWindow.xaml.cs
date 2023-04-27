using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp_lab_4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window {
        FillRule rule = FillRule.EvenOdd;
        PathFigure currentFigure;
        Path currentPath;
        bool isDrawing = false;
        Point lastPoint;
        public MainWindow() {
            InitializeComponent();
        }

        void DrawingMouseDown(object sender, MouseButtonEventArgs e) {
            if (!isDrawing) {
                Mouse.Capture(DrawingTarget);
                isDrawing = true;
                StartFigure(e.GetPosition(DrawingTarget));
            }
            AddFigurePoint(e.GetPosition(DrawingTarget));
        }

        void StartFigure(Point start) {
            currentFigure = new PathFigure() { StartPoint = start };
            if ((bool)NonzeroRB.IsChecked)
                rule = FillRule.Nonzero;
            else
                rule = FillRule.EvenOdd;
            currentPath =
                new Path() {
                    Stroke = Brushes.YellowGreen,
                    StrokeThickness = 3,
                    Data = new PathGeometry() { Figures = { currentFigure }, FillRule = rule }
                };
            DrawingTarget.Children.Add(currentPath);
        }

        void AddFigurePoint(Point point) {
            if (!point.Equals(lastPoint)) {
                currentFigure.Segments.Add(new LineSegment(point, isStroked: true));
                lastPoint = new Point(point.X, point.Y);
            } else {
                EndFigure();
                isDrawing = false;
                Mouse.Capture(null);
            }
        }

        void EndFigure() {
            currentFigure.IsClosed = true;
            currentPath.Fill = new SolidColorBrush(Colors.Yellow);
            currentFigure = null;
        }

        private void NonzeroRB_Checked(object sender, RoutedEventArgs e) {
            evenOddRB.IsChecked = false;
        }

        private void evenOddRB_Checked(object sender, RoutedEventArgs e) {
            NonzeroRB.IsChecked = false;
        }
    }
}
