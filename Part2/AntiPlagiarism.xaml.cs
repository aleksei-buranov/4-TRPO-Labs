using System;
using System.Linq;
using System.Windows;

namespace Part2
{
    /// <summary>
    /// Interaction logic for AntiPlagiarism.xaml
    /// </summary>
    public partial class AntiPlagiarism : Window
    {
        public AntiPlagiarism()
        {
            InitializeComponent();
        }

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(MinPercentage.Text, out var minPercentage))
                return;
            var texts1 = TextBox1.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .ToHashSet();
            var texts2 = TextBox2.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .ToHashSet();
            var commonStrings = texts1.Intersect(texts2).ToList();
            var uniqueStrings = texts1.Concat(texts2).ToHashSet();
            var percentage = (double)commonStrings.Count / uniqueStrings.Count * 100;
            var percentageString = percentage.ToString("F2") + "%";
            var resultString = percentageString;
            if (percentage >= minPercentage)
                resultString += ", плагиат";
            ResultTextBlock.Text = resultString;
        }
    }
}
