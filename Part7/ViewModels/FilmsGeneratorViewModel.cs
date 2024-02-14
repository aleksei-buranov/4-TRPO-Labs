using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;

namespace Part78.ViewModels
{
    public class FilmsGeneratorViewModel : ObservableObject
    {
        private readonly SqliteConnection _sqliteConnection;

        public FilmsGeneratorViewModel(SqliteConnection sqliteConnection)
        {
            _sqliteConnection = sqliteConnection;
            Films = new ObservableCollection<FilmViewModel>();
        }

        public ObservableCollection<FilmViewModel> Films { get; }
        public string Condition { get; set; }

        public RelayCommand RunSelectQueryCommand => new RelayCommand(UpdateFilms);
        public RelayCommand RunModifyQueryCommand => new RelayCommand(UpdateFilmsAfterReplace);

        private void UpdateFilmsAfterReplace()
        {
            var films = RunSelectQuery();
            if (films == null)
                return;
            var oldFilms = films.ToList();
            var newFilms = new List<FilmViewModel>();
            foreach (var filmViewModel in oldFilms)
            {
                var newTitleArray = filmViewModel.FilmName.Reverse().ToArray();
                newTitleArray[0] = char.ToUpper(newTitleArray[0]);
                newTitleArray[newTitleArray.Length - 1] = char.ToLower(newTitleArray[newTitleArray.Length - 1]);
                var newTitle = string.Join(string.Empty, newTitleArray);

                var newFilmViewModel = new FilmViewModel
                {
                    FilmId = filmViewModel.FilmId,
                    FilmName = newTitle,
                    FilmYear = filmViewModel.FilmYear + 1000,
                    FilmGenre = filmViewModel.FilmGenre,
                    FilmDuration = filmViewModel.FilmDuration * 2
                };

                newFilms.Add(newFilmViewModel);
            }

            RunDeleteQuery(oldFilms.Select(f => f.FilmId));
            RunInsertQuery(newFilms);
            UpdateFilms();
        }

        private void RunDeleteQuery(IEnumerable<int> ids)
        {
            using (_sqliteConnection)
            {
                _sqliteConnection.Open();

                foreach (var id in ids)
                {
                    var command =
                        new SqliteCommand(
                            $"Delete from films where id = {id}",
                            _sqliteConnection);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void RunInsertQuery(IEnumerable<FilmViewModel> newFilms)
        {
            using (_sqliteConnection)
            {
                _sqliteConnection.Open();

                foreach (var filmViewModel in newFilms)
                {
                    var command =
                        new SqliteCommand(
                            "Insert INTO films (id, title, year, genre, duration) " +
                            $"VALUES ({filmViewModel.FilmId}, '{filmViewModel.FilmName}', " +
                            $"{filmViewModel.FilmYear}, {filmViewModel.FilmGenre}, " +
                            $"{filmViewModel.FilmDuration})",
                            _sqliteConnection);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateFilms()
        {
            var items = RunSelectQuery()?.ToList();
            if (items == null)
                return;
            Films.Clear();
            foreach (var filmViewModel in items) Films.Add(filmViewModel);
        }

        private IEnumerable<FilmViewModel> RunSelectQuery()
        {
            return string.IsNullOrWhiteSpace(Condition) ? null : RunSelectQueryInternal();
        }

        private IEnumerable<FilmViewModel> RunSelectQueryInternal()
        {
            var result = new List<FilmViewModel>();

            using (_sqliteConnection)
            {
                _sqliteConnection.Open();

                var command =
                    new SqliteCommand(
                        $"Select * From films where {Condition}",
                        _sqliteConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        while (reader.Read())
                            yield return new FilmViewModel(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2),
                                reader.GetInt32(3), reader.GetInt32(4));
                }
            }
        }
    }
}
