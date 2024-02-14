using System.Globalization;
using System.Windows;

namespace Part1
{
    /// <summary>
    /// Interaction logic for MiniCalc.xaml
    /// </summary>
    public partial class MiniCalc : Window
    {
        public MiniCalc()
        {
            InitializeComponent();
            TrickButton.Content = "->";
            FirstValue.Text = "12";
            SecondValue.Text = "3";
        }

        private void TrickButton_Click(object sender, RoutedEventArgs e)
        {
            var firstValue = double.Parse(FirstValue.Text);
            var secondValue = double.Parse(SecondValue.Text);
            double sumResult, subResult, multiplyResult, divideResult;

            if (double.IsNaN(firstValue) || double.IsNaN(secondValue))
            {
                sumResult = subResult = multiplyResult = divideResult = default;
            }
            else
            {
                sumResult = firstValue + secondValue;
                subResult = firstValue - secondValue;
                multiplyResult = firstValue * secondValue;
                divideResult = secondValue == 0 ? double.NaN : firstValue / secondValue;
            }

            SumResult.Text = sumResult.ToString(CultureInfo.InvariantCulture);
            SubResult.Text = subResult.ToString(CultureInfo.InvariantCulture);
            MultiplyResult.Text = multiplyResult.ToString(CultureInfo.InvariantCulture);
            DivideResult.Text = double.IsNaN(divideResult)
                ? "Error"
                : divideResult.ToString(CultureInfo.InvariantCulture);
        }
    }
}
