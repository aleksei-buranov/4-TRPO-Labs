using CommunityToolkit.Mvvm.ComponentModel;

namespace Part78.ViewModels
{
    public class FilmViewModel : ObservableObject
    {
        private int _filmId;
        private string _filmName;
        private int _filmYear;
        private int _filmGenre;
        private int _filmDuration;
        private string _filmGenreLabel;

        public FilmViewModel()
        {
        }

        public FilmViewModel(int filmId, string filmName, int filmYear, int filmGenre, int filmDuration)
        {
            FilmId = filmId;
            FilmName = filmName;
            FilmYear = filmYear;
            FilmGenre = filmGenre;
            FilmDuration = filmDuration;
        }

        public int FilmId
        {
            get => _filmId;
            set
            {
                if (value == _filmId) return;
                _filmId = value;
                OnPropertyChanged();
            }
        }

        public string FilmName
        {
            get => _filmName;
            set
            {
                if (value == _filmName) return;
                _filmName = value;
                OnPropertyChanged();
            }
        }

        public int FilmYear
        {
            get => _filmYear;
            set
            {
                if (value == _filmYear) return;
                _filmYear = value;
                OnPropertyChanged();
            }
        }

        public int FilmGenre
        {
            get => _filmGenre;
            set
            {
                if (value == _filmGenre) return;
                _filmGenre = value;
                OnPropertyChanged();
            }
        }

        public int FilmDuration
        {
            get => _filmDuration;
            set
            {
                if (value == _filmDuration) return;
                _filmDuration = value;
                OnPropertyChanged();
            }
        }

        public string FilmGenreLabel
        {
            get => _filmGenreLabel;
            set
            {
                if (value == _filmGenreLabel) return;
                _filmGenreLabel = value;
                OnPropertyChanged();
            }
        }
    }
}
