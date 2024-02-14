using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;

namespace Part78.ViewModels
{
    internal class GenreWindowViewModel : ObservableObject
    {
        private readonly SqliteConnection _connection;
        private string _selectedGenre;

        public GenreWindowViewModel(SqliteConnection connection)
        {
            _connection = connection;
            Genres = new ObservableCollection<string>();
            Films = new ObservableCollection<FilmViewModel>();
            FillGenres();
            SelectedGenre = Genres.FirstOrDefault();
        }

        public RelayCommand FilterFilmsCommand => new RelayCommand(FilterFilms);

        public ObservableCollection<string> Genres { get; }

        public ObservableCollection<FilmViewModel> Films { get; }

        public string SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                if (value == _selectedGenre) return;
                _selectedGenre = value;
                OnPropertyChanged();
            }
        }

        private void FilterFilms()
        {
            if (SelectedGenre == null)
                return;

            Films.Clear();
            using (_connection)
            {
                _connection.Open();
                var command = new SqliteCommand("Select title, genre, year From films " +
                                                "Where genre = (Select id From genres " +
                                                $"Where title = \'{SelectedGenre}\')", _connection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return;
                    while (reader.Read())
                        Films.Add(new FilmViewModel
                        {
                            FilmName = reader.GetString(0),
                            FilmGenre = reader.GetInt32(1),
                            FilmYear = reader.GetInt32(2)
                        });
                }
            }
        }

        private void FillGenres()
        {
            using (_connection)
            {
                _connection.Open();
                var command = new SqliteCommand("Select title From genres Order By title ASC", _connection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return;
                    while (reader.Read())
                        Genres.Add(reader.GetString(0));
                }
            }
        }
    }
}
