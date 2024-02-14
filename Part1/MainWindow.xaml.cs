using System.Windows;

namespace Part1
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowWindowDialog<T>() where T : Window, new()
        {
            var t = new T
            {
                Owner = this
            };
            t.ShowDialog();
        }

        private void ShowFocusWindowButton_Click(object sender, RoutedEventArgs e)
        {
            ShowWindowDialog<WordTrickWindow>();
        }

        private void ShowEvaluateExpressionButton_Click(object sender, RoutedEventArgs e)
        {
            ShowWindowDialog<ExpressionEvaluator>();
        }

        private void ShowMiniCalcButton_Click(object sender, RoutedEventArgs e)
        {
            ShowWindowDialog<MiniCalc>();
        }

        private void ShowAriphmometerButton_Click(object sender, RoutedEventArgs e)
        {
            ShowWindowDialog<Ariphmometer>();
        }

        private void ShowOrderButton_Click(object sender, RoutedEventArgs e)
        {
            ShowWindowDialog<OrderWindow>();
        }
    }
}