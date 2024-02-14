using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;

namespace Part78.ViewModels
{
    public class FilmsLibraryViewModel : ObservableObject
    {
        private readonly SqliteConnection _sqliteConnection;
        private FilmViewModel _selectedFilm;
        private GenreViewModel _selectedGenre;
        private TabItem _selectedTab;

        public FilmsLibraryViewModel(SqliteConnection sqliteConnection)
        {
            _sqliteConnection = sqliteConnection;
            Films = new ObservableCollection<FilmViewModel>();
            Genres = new ObservableCollection<GenreViewModel>();
            FillGenres();
            UpdateFilms();
        }

        public ObservableCollection<GenreViewModel> Genres { get; }

        public ObservableCollection<FilmViewModel> Films { get; }
        public RelayCommand<object> RunAddFilmCommand => new RelayCommand<object>(AddNewFilm);

        public RelayCommand<object> RunEditFilmCommand => new RelayCommand<object>(EditFilm,
            o => SelectedFilm != null);

        public RelayCommand RunDeleteFilmCommand => new RelayCommand(DeleteFilm,
            () => SelectedFilm != null);

        public RelayCommand<object> RunAddGenreCommand => new RelayCommand<object>(AddNewGenre);

        public RelayCommand<object> RunEditGenreCommand =>
            new RelayCommand<object>(EditGenre, o => SelectedGenre != null);

        public RelayCommand RunDeleteGenreCommand => new RelayCommand(DeleteGenre,
            () => SelectedGenre != null);

        public FilmViewModel SelectedFilm
        {
            get => _selectedFilm;
            set
            {
                if (Equals(value, _selectedFilm)) return;
                _selectedFilm = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RunEditFilmCommand));
                OnPropertyChanged(nameof(RunDeleteFilmCommand));
            }
        }

        public GenreViewModel SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                if (Equals(value, _selectedGenre)) return;
                _selectedGenre = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RunEditGenreCommand));
                OnPropertyChanged(nameof(RunDeleteGenreCommand));
            }
        }

        public TabItem SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (Equals(value, _selectedTab)) return;
                _selectedTab = value;
                OnPropertyChanged();
                switch ((string)SelectedTab.Header)
                {
                    case "Фильмы":
                        UpdateFilms();
                        break;
                    case "Жанры":
                        FillGenres();
                        break;
                }
            }
        }

        #region Films

        private void AddNewFilm(object o)
        {
            var vm = new FilmEditorViewModel(Genres.Select(g => g.Title));
            var editorWindow = new FilmEditorWindow
            {
                DataContext = vm
            };
            if (o is Window window)
                editorWindow.Owner = window;
            editorWindow.ShowDialog();
            if (!vm.IsDataValid)
                return;
            var newFilm = new FilmViewModel
            {
                FilmName = vm.Title,
                FilmDuration = vm.Duration,
                FilmYear = vm.Year,
                FilmGenre = Genres.First(g => g.Title == vm.SelectedGenre).Id
            };
            RunInsertFilmQuery(newFilm);
            UpdateFilms();
        }

        private void EditFilm(object o)
        {
            var vm = new FilmEditorViewModel(Genres.Select(g => g.Title), SelectedFilm);
            var editorWindow = new FilmEditorWindow
            {
                DataContext = vm
            };
            if (o is Window window)
                editorWindow.Owner = window;
            editorWindow.ShowDialog();
            if (!vm.IsDataValid)
                return;
            var newFilm = new FilmViewModel
            {
                FilmId = SelectedFilm.FilmId,
                FilmName = vm.Title,
                FilmDuration = vm.Duration,
                FilmYear = vm.Year,
                FilmGenre = Genres.First(g => g.Title == vm.SelectedGenre).Id
            };
            if (SelectedFilm.FilmName == newFilm.FilmName && SelectedFilm.FilmDuration == newFilm.FilmDuration &&
                SelectedFilm.FilmYear == newFilm.FilmYear && SelectedFilm.FilmGenre == newFilm.FilmGenre)
                return;
            RunUpdateFilmQuery(newFilm);
            UpdateFilms();
        }

        private void DeleteFilm()
        {
            var res = MessageBox.Show($"Вы действительно хотите удалить фильм '{SelectedFilm.FilmName}'?",
                "Удаление фильма",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.Yes)
                return;
            RunDeleteFilmQuery(SelectedFilm);
            UpdateFilms();
        }

        private void UpdateFilms()
        {
            var items = RunSelectFilmsQuery().ToList();
            Films.Clear();
            foreach (var filmViewModel in items) Films.Add(filmViewModel);
        }

        private IEnumerable<FilmViewModel> RunSelectFilmsQuery()
        {
            using (_sqliteConnection)
            {
                _sqliteConnection.Open();

                var command =
                    new SqliteCommand("Select * From films", _sqliteConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        while (reader.Read())
                            yield return new FilmViewModel(reader.GetInt32(0),
                                reader.GetString(1), reader.GetInt32(2),
                                reader.GetInt32(3), reader.GetInt32(4))
                            {
                                FilmGenreLabel = Genres.FirstOrDefault(g => g.Id == reader.GetInt32(3))?.Title
                            };
                }
            }
        }

        private void RunInsertFilmQuery(FilmViewModel filmViewModel)
        {
            using (_sqliteConnection)
            {
                _sqliteConnection.Open();

                var command =
                    new SqliteCommand("Insert into films (title, year, genre, duration) values " +
                                      $"('{filmViewModel.FilmName}', {filmViewModel.FilmYear}, {filmViewModel.FilmGenre}, {filmViewModel.FilmDuration})",
                        _sqliteConnection);
                command.ExecuteNonQuery();
            }
        }

        private void RunUpdateFilmQuery(FilmViewModel filmViewModel)
        {
            using (_sqliteConnection)
            {
                _sqliteConnection.Open();

                var command =
                    new SqliteCommand($"Update films set title = '{filmViewModel.FilmName}', " +
                                      $"year = {filmViewModel.FilmYear}, " +
                                      $"genre = {filmViewModel.FilmGenre}, " +
                                      $"duration = {filmViewModel.FilmDuration} " +
                                      $"where id = {filmViewModel.FilmId}",
                        _sqliteConnection);
                command.ExecuteNonQuery();
            }
        }

        private void RunDeleteFilmQuery(FilmViewModel filmViewModel)
        {
            using (_sqliteConnection)
            {
                _sqliteConnection.Open();

                var command =
                    new SqliteCommand($"Delete from films where id = {filmViewModel.FilmId}",
                        _sqliteConnection);
                command.ExecuteNonQuery();
            }
        }

        #endregion

        #region Genres

        private void AddNewGenre(object obj)
        {
            var vm = new GenreEditorViewModel();
            var editorWindow = new GenreEditorWindow
            {
                DataContext = vm
            };
            if (obj is Window window)
                editorWindow.Owner = window;
            editorWindow.ShowDialog();
            if (!vm.IsDataValid)
                return;
            var newGenre = new GenreViewModel(default, vm.Title);
            RunInsertGenreQuery(newGenre);
            FillGenres();
        }

        private void EditGenre(object obj)
        {
            var vm = new GenreEditorViewModel(SelectedGenre);
            var editorWindow = new GenreEditorWindow
            {
                DataContext = vm
            };
            if (obj is Window window)
                editorWindow.Owner = window;
            editorWindow.ShowDialog();
            if (!vm.IsDataValid)
                return;
            if (SelectedGenre.Title == vm.Title)
                return;
            var newGenre = new GenreViewModel(SelectedGenre.Id, vm.Title);
            RunUpdateGenreQuery(newGenre);
            FillGenres();
        }

        private void DeleteGenre()
        {
            var res = MessageBox.Show($"Вы действительно хотите удалить жанр '{SelectedGenre.Title}'?",
                "Удаление жанра",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.Yes)
                return;
            RunDeleteGenreQuery(SelectedGenre);
            FillGenres();
        }

        private void FillGenres()
        {
            Genres.Clear();
            using (_sqliteConnection)
            {
                _sqliteConnection.Open();
                var command = new SqliteCommand("Select * From genres", _sqliteConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return;
                    while (reader.Read())
                        Genres.Add(new GenreViewModel(reader.GetInt32(0), reader.GetString(1)));
                }
            }
        }

        private void RunInsertGenreQuery(GenreViewModel genreViewModel)
        {
            using (_sqliteConnection)
            {
                _sqliteConnection.Open();

                var command =
                    new SqliteCommand("Insert into genres (title) values " +
                                      $"('{genreViewModel.Title}')",
                        _sqliteConnection);
                command.ExecuteNonQuery();
            }
        }

        private void RunUpdateGenreQuery(GenreViewModel genreViewModel)
        {
            using (_sqliteConnection)
            {
                _sqliteConnection.Open();

                var command =
                    new SqliteCommand($"Update genres set title = '{genreViewModel.Title}' " +
                                      $"where id = {genreViewModel.Id}",
                        _sqliteConnection);
                command.ExecuteNonQuery();
            }
        }

        private void RunDeleteGenreQuery(GenreViewModel genreViewModel)
        {
            using (_sqliteConnection)
            {
                _sqliteConnection.Open();

                var command =
                    new SqliteCommand($"Delete from genres where id = {genreViewModel.Id}",
                        _sqliteConnection);
                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
