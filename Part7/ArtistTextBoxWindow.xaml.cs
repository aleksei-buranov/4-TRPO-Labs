using System.Windows;

namespace Part78
{
    /// <summary>
    /// Interaction logic for ArtistTextBoxWindow.xaml
    /// </summary>
    public partial class ArtistTextBoxWindow : Window
    {
        public static readonly DependencyProperty ArtistNameProperty = DependencyProperty.Register(
            nameof(ArtistName), typeof(string), typeof(ArtistTextBoxWindow), new PropertyMetadata(default(string)));

        public string ArtistName
        {
            get => (string)GetValue(ArtistNameProperty);
            set => SetValue(ArtistNameProperty, value);
        }

        public ArtistTextBoxWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ArtistName = ArtistTextBox.Text;
            Close();
        }
    }
}
