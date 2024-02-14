using System.Windows;

namespace Part1
{
    /// <summary>
    ///     Interaction logic for WordTrickWindow.xaml
    /// </summary>
    public partial class WordTrickWindow : Window
    {
        public WordTrickWindow()
        {
            InitializeComponent();
            TrickButton.Content = "<-";
            SecondValue.Text = "Фокус";
        }

        private void TrickButton_Click(object sender, RoutedEventArgs e)
        {
            (FirstValue.Text, SecondValue.Text) = (SecondValue.Text, FirstValue.Text);
            TrickButton.Content = (string)TrickButton.Content == "<-" ? "->" : "<-";
        }
    }
}
