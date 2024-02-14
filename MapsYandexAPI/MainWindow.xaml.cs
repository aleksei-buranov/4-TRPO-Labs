using System;
using System.Windows;
using System.Windows.Input;
using MapsYandexAPI.ViewModels;

namespace MapsYandexAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainVM;

        public MainWindow()
        {
            _mainVM = new MainViewModel();
            DataContext = _mainVM;
            InitializeComponent();
        }

        private void MainWindow_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.PageUp:
                    _mainVM.Zoom++;
                    break;
                case Key.PageDown:
                    _mainVM.Zoom--;
                    break;
                case Key.Left:
                    _mainVM.Longitude -= MainViewModel.AddStep * Math.Pow(2, 15 - _mainVM.Zoom);
                    break;
                case Key.Right:
                    _mainVM.Longitude += MainViewModel.AddStep * Math.Pow(2, 15 - _mainVM.Zoom);
                    break;
                case Key.Up:
                    if (_mainVM.Latitude < 85)
                        _mainVM.Latitude += MainViewModel.AddStep * Math.Pow(2, 15 - _mainVM.Zoom);
                    break;
                case Key.Down:
                    if (_mainVM.Latitude > -85)
                        _mainVM.Latitude -= MainViewModel.AddStep * Math.Pow(2, 15 - _mainVM.Zoom);
                    break;
            }
        }
    }
}
