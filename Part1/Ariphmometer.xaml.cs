using System.Globalization;
using System.Windows;

namespace Part1
{
    /// <summary>
    /// Interaction logic for Ariphmometer.xaml
    /// </summary>
    public partial class Ariphmometer : Window
    {
        public Ariphmometer()
        {
            InitializeComponent();
            FirstValue.Text = "0";
            SecondValue.Text = "0";
            ResultValue.Text = "0";
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(FirstValue.Text, out var firstValue))
                return;
            if (!double.TryParse(SecondValue.Text, out var secondValue))
                return;

            ResultValue.Text = (firstValue + secondValue).ToString(CultureInfo.InvariantCulture);
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(FirstValue.Text, out var firstValue))
                return;
            if (!double.TryParse(SecondValue.Text, out var secondValue))
                return;

            ResultValue.Text = (firstValue - secondValue).ToString(CultureInfo.InvariantCulture);
        }

        private void MultiplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(FirstValue.Text, out var firstValue))
                return;
            if (!double.TryParse(SecondValue.Text, out var secondValue))
                return;

            ResultValue.Text = (firstValue * secondValue).ToString(CultureInfo.InvariantCulture);
        }
    }
}
