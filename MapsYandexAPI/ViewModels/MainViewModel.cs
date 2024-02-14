using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Yandex.Geocoder;
using Yandex.Maps.StaticAPI;
using Yandex.Maps.StaticAPI.PT;

namespace MapsYandexAPI.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        public const double AddStep = 0.004;
        private BitmapImage _currentImage;
        private bool _isHybridLayoutSelected;
        private bool _isSatelliteLayoutSelected;
        private bool _isSchemaLayoutSelected;
        private double _latitude;
        private double _longitude;
        private string _searchText;
        private int _zoom;

        public MainViewModel()
        {
            _latitude = 51.2205;
            _longitude = 58.4770;
            _isSchemaLayoutSelected = true;
            _zoom = 15;
            LastPoint = null;
            Geocoder = new YandexGeocoder
            {
                Apikey = "430f609a-77d7-4a75-b0e6-36fc17e5abc4",
                LanguageCode = LanguageCode.ru_RU,
                Results = 1
            };

            UpdateApi();
            UpdateImage();
        }

        private static Lang Lang => new Lang(Lang.Lang_reg.ru_RU);
        private static Size Size => new Size(450, 450);
        private L Layout => new L(IsSchemaLayoutSelected, IsSatelliteLayoutSelected, IsHybridLayoutSelected, false);
        private YandexGeocoder Geocoder { get; }

        private StaticAPI Api { get; set; }
        private Pt LastPoint { get; set; }
        private string LastPointAddress { get; set; }


        public double Latitude
        {
            get => _latitude;
            set
            {
                if (value.Equals(_latitude)) return;
                _latitude = value;
                OnPropertyChanged();
                ReInitializeImage();
            }
        }

        public double Longitude
        {
            get => _longitude;
            set
            {
                if (value.Equals(_longitude)) return;
                _longitude = value;
                OnPropertyChanged();
                ReInitializeImage();
            }
        }

        public BitmapImage CurrentImage
        {
            get => _currentImage;
            set
            {
                if (Equals(value, _currentImage)) return;
                _currentImage = value;
                OnPropertyChanged();
            }
        }

        public int Zoom
        {
            get => _zoom;
            set
            {
                if (value == _zoom || value > 17 || value < 0) return;
                _zoom = value;
                OnPropertyChanged();
                Api.Z = Zoom;
                UpdateImage();
            }
        }

        public bool IsSchemaLayoutSelected
        {
            get => _isSchemaLayoutSelected;
            set
            {
                if (value == _isSchemaLayoutSelected) return;
                _isSchemaLayoutSelected = value;
                OnPropertyChanged();
                if (value)
                    ReInitializeImage();
            }
        }

        public bool IsSatelliteLayoutSelected
        {
            get => _isSatelliteLayoutSelected;
            set
            {
                if (value == _isSatelliteLayoutSelected) return;
                _isSatelliteLayoutSelected = value;
                OnPropertyChanged();
                if (value)
                    ReInitializeImage();
            }
        }

        public bool IsHybridLayoutSelected
        {
            get => _isHybridLayoutSelected;
            set
            {
                if (value == _isHybridLayoutSelected) return;
                _isHybridLayoutSelected = value;
                OnPropertyChanged();
                if (value)
                    ReInitializeImage();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value == _searchText) return;
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public AsyncRelayCommand SearchCommand => new AsyncRelayCommand(Search);
        public RelayCommand ResetPointCommand => new RelayCommand(ResetPoint);

        private async Task Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return;
            Geocoder.SearchQuery = SearchText;
            var response = await Geocoder.GetResultsAsync();
            var result = response.FirstOrDefault();
            if (result == null)
                return;
            Latitude = result.Point.Latitude;
            Longitude = result.Point.Longitude;
            LastPoint = new Pt(new Pm2(Latitude, Longitude));

            ReInitializeImage();
        }

        private void ResetPoint()
        {
            LastPoint = null;
            LastPointAddress = string.Empty;
            ReInitializeImage();
        }

        private void ReInitializeImage()
        {
            UpdateApi();
            UpdateImage();
        }

        private void UpdateApi()
        {
            Api = new StaticAPI(Layout, new LL(Latitude, Longitude), Zoom, Size, Lang, pt: LastPoint);
        }

        private void UpdateImage()
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Api.GetPictureURL(), UriKind.Absolute);
            bitmap.EndInit();

            CurrentImage = null;
            CurrentImage = bitmap;
        }
    }
}