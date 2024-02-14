using System.Windows;

namespace Part2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
            ShowWindowDialog<MiniPlanner>();
        }

        private void ShowAntiPlagiarismButton_Click(object sender, RoutedEventArgs e)
        {
            ShowWindowDialog<AntiPlagiarism>();
        }

        private void ShowNoteBookButton_Click(object sender, RoutedEventArgs e)
        {
            ShowWindowDialog<Notebook>();
        }
    }
}
