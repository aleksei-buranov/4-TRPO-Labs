using System.Text.RegularExpressions;
using System.Windows;

namespace Part2
{
    /// <summary>
    /// Interaction logic for Notebook.xaml
    /// </summary>
    public partial class Notebook : Window
    {
        public Notebook()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var number = PhoneTextBox.Text;
            var name = NameTextBox.Text;
            if (!Regex.IsMatch(number, "\\d*") || string.IsNullOrEmpty(name))
                return;
            var newItem = $"{name} {number}";
            if (NotesListBox.Items.Contains(newItem))
                return;
            NotesListBox.Items.Add(newItem);
        }
    }
}
