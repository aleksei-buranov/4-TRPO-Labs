using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;

namespace Part78.ViewModels
{
    public class AlphaviteFilmsViewModel : ObservableObject
    {
        private readonly SqliteConnection _sqliteConnection;
        private string _filmsCount;
        private const string FilmsCountTemplate = "Нашлось {0} записей";
        private char _currentLetter;

        public AlphaviteFilmsViewModel(SqliteConnection sqliteConnection)
        {
            _sqliteConnection = sqliteConnection;
            Films = new ObservableCollection<FilmViewModel>();
            FilmsCount = string.Empty;
            _currentLetter = default;
        }

        public ObservableCollection<FilmViewModel> Films { get; }

        public RelayCommand<char> RunFiltrationCommand => new RelayCommand<char>(FilterByLetter);

        public string FilmsCount
        {
            get => _filmsCount;
            set
            {
                if (value == _filmsCount) return;
                _filmsCount = value;
                OnPropertyChanged();
            }
        }

        private void FilterByLetter(char obj)
        {
            if (obj == _currentLetter)
                return;
            _currentLetter = obj;
            FilmsCount = string.Empty;
            Films.Clear();
            using (_sqliteConnection)
            {
                _sqliteConnection.Open();
                var lowerChar = char.ToLower(obj);
                var upperChar = char.ToUpper(obj);

                var command =
                    new SqliteCommand(
                        $"Select * From films where title like '{lowerChar}%' or title like '{upperChar}%'",
                        _sqliteConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        while (reader.Read())
                            Films.Add(new FilmViewModel(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2),
                                reader.GetInt32(3), reader.GetInt32(4)));
                }
            }

            FilmsCount = string.Format(FilmsCountTemplate, Films.Count);
        }
    }
}
