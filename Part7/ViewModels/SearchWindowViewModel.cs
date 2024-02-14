using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;
using Part78.Const;

namespace Part78.ViewModels
{
    internal class SearchWindowViewModel : ObservableObject
    {
        private readonly SqliteConnection _connection;
        private string _searchLabel;
        private string _searchText;
        private string _errorText;
        private int _filmId;
        private string _filmName;
        private int _filmYear;
        private int _filmGenre;
        private int _filmDuration;

        public SearchWindowViewModel(SqliteConnection connection)
        {
            _connection = connection;
            SearchLabel = SearchLabelsList.First();
            _errorText = string.Empty;
        }

        public List<string> SearchLabelsList => Common.SearchLabelsList;
        public RelayCommand SearchCommand => new RelayCommand(Search);

        public string SearchLabel
        {
            get => _searchLabel;
            set
            {
                if (value == _searchLabel) return;
                _searchLabel = value;
                OnPropertyChanged();
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

        public string ErrorText
        {
            get => _errorText;
            private set
            {
                if (value == _errorText) return;
                _errorText = value;
                OnPropertyChanged();
            }
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

        private void Search()
        {
            ErrorText = string.Empty;
            double doubleValue = default;
            if ((SearchLabel == Common.YearLabel || SearchLabel == Common.DurationLabel) &&
                !double.TryParse(SearchText, out doubleValue))
            {
                ErrorText = "Неправильный запрос";
                return;
            }

            var requestTemplate = "Select * from Films " +
                                  "Where {0} = {1} " +
                                  "Order by Id ASC";
            var request = string.Empty;

            switch (SearchLabel)
            {
                case Common.NameLabel:
                    request = string.Format(requestTemplate, "title", $"\'{SearchText}\'");
                    break;
                case Common.YearLabel:
                    request = string.Format(requestTemplate, "year", doubleValue);
                    break;
                case Common.DurationLabel:
                    request = string.Format(requestTemplate, "duration", doubleValue);
                    break;
            }

            using (_connection)
            {
                _connection.Open();
                var command = new SqliteCommand(request, _connection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        ErrorText = "Ничего не найдено";
                        return;
                    }

                    reader.Read();
                    FilmId = reader.GetInt32(0);
                    FilmName = reader.GetString(1);
                    FilmYear = reader.GetInt32(2);
                    FilmGenre = reader.GetInt32(3);
                    FilmDuration = reader.GetInt32(4);
                }
            }
        }
    }
}
