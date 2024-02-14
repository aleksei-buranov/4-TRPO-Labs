using System.Text;
using System.Windows;

namespace Part1
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();
            OrderPanel.Visibility = Visibility.Hidden;
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            if (CheeseBurgerCheckBox.IsChecked == true) sb.AppendLine((string)CheeseBurgerCheckBox.Content);
            if (HamburgerCheckBox.IsChecked == true) sb.AppendLine((string)HamburgerCheckBox.Content);
            if (NuggetsCheckBox.IsChecked == true) sb.AppendLine((string)NuggetsCheckBox.Content);
            if (ColaCheckBox.IsChecked == true) sb.AppendLine((string)ColaCheckBox.Content);
            var orderResult = sb.ToString();
            if (!string.IsNullOrEmpty(orderResult))
            {
                OrderPanel.Visibility = Visibility.Visible;
                OrderTextBox.Text = orderResult;
            }
            else
            {
                OrderPanel.Visibility = Visibility.Hidden;
            }
        }
    }
}
