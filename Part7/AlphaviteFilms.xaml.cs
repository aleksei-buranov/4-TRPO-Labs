using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Part78.ViewModels;

namespace Part78
{
    /// <summary>
    /// Interaction logic for AlphaviteFilms.xaml
    /// </summary>
    public partial class AlphaviteFilms : Window
    {
        private readonly AlphaviteFilmsViewModel _vm;

        public AlphaviteFilms(AlphaviteFilmsViewModel vm)
        {
            _vm = vm;
            DataContext = vm;
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var letter = (sender as Button).Content.ToString().First();
            _vm.RunFiltrationCommand.Execute(letter);
        }
    }
}
