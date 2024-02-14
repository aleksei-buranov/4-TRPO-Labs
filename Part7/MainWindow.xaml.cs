using System;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Data.Sqlite;
using Part78.Properties;
using Part78.ViewModels;

namespace Part78
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SqliteConnection FilmsDbConnection { get; }
        public SqliteConnection MusicStoreConnection { get; }

        public MainWindow()
        {
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            FilmsDbConnection =
                new SqliteConnection($"Data Source={projectDirectory}\\{Settings.Default.FilmsDbConnectionString}");
            MusicStoreConnection =
                new SqliteConnection($"Data Source={projectDirectory}\\{Settings.Default.MusicStoreConnectionString}");
            InitializeComponent();
        }

        private void ShowNewFilmsButton_Click(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();

            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand("SELECT title From Films " +
                                                "Where genre = (Select id from genres " +
                                                "Where title = \'музыка\' OR title =  \'анимация\') " +
                                                "AND year > 1997", FilmsDbConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return;
                    while (reader.Read())
                        stringBuilder.AppendLine(reader.GetString(0));
                }
            }

            MessageBox.Show(stringBuilder.ToString(), "Новинки анимации и музыки (после 1997 года)");
        }

        private void ShowQuestionFilmsButton_Click(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();

            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand("SELECT title From Films Where title like \'%?\'", FilmsDbConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return;
                    while (reader.Read())
                        stringBuilder.AppendLine(reader.GetString(0));
                }
            }

            MessageBox.Show(stringBuilder.ToString(), "Фильмы с вопросами на конце названий");
        }

        private void ShowImportantYears_Click(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();

            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand("SELECT Distinct year From Films Where title like \'Х%\' Order By year",
                    FilmsDbConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return;
                    while (reader.Read())
                        stringBuilder.AppendLine(reader.GetString(0));
                }
            }

            MessageBox.Show(stringBuilder.ToString(), "Годы выхода фильмов, у которых названия начинаются с Х");
        }

        private void ShowLongComediesButton_Click(object sender, RoutedEventArgs e)
        {
            var longComedies = new LongComedies(FilmsDbConnection)
            {
                Owner = this
            };
            longComedies.ShowDialog();
        }

        private void ShowSearchWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var searchWindow = new SearchWindow
            {
                DataContext = new SearchWindowViewModel(FilmsDbConnection),
                Owner = this
            };
            searchWindow.ShowDialog();
        }

        private void GenreFilterWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new GenreFiltration
            {
                DataContext = new GenreWindowViewModel(FilmsDbConnection),
                Owner = this
            };
            window.ShowDialog();
        }

        private void TracklistWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new ArtistTextBoxWindow
            {
                Owner = this
            };
            window.ShowDialog();
            var artist = window.ArtistName;
            if (string.IsNullOrEmpty(artist))
                return;
            var stringBuilder = new StringBuilder();

            using (MusicStoreConnection)
            {
                MusicStoreConnection.Open();
                var command = new SqliteCommand("SELECT DISTINCT Track.Name " +
                                                "FROM Track " +
                                                "JOIN Album ON Track.AlbumId = Album.AlbumId " +
                                                "JOIN Artist ON Album.ArtistId = Artist.ArtistId " +
                                                $"WHERE Artist.Name = \'{artist}\' " +
                                                "ORDER BY Track.Name ASC",
                    MusicStoreConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return;
                    if (reader.Read())
                        stringBuilder.Append(reader.GetString(0));

                    while (reader.Read())
                        stringBuilder.Append("; " + reader.GetString(0));
                }
            }

            MessageBox.Show(stringBuilder.ToString(), "Список треков");
        }

        private void OldDetectivesWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();

            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand("Select title from films " +
                                                "where genre = (SELECT id from genres where title = 'детектив') AND " +
                                                "year BETWEEN 1995 AND 2000",
                    FilmsDbConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return;
                    if (reader.Read())
                        stringBuilder.Append(reader.GetString(0));

                    while (reader.Read())
                        stringBuilder.Append("; " + reader.GetString(0));
                }
            }

            MessageBox.Show(stringBuilder.ToString(), "Старые детективы");
        }

        private void PopularGenresWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();

            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand("Select Distinct genres.title " +
                                                "from genres " +
                                                "join films on genres.id = films.genre " +
                                                "where films.year in (2010, 2011)",
                    FilmsDbConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return;

                    while (reader.Read())
                        stringBuilder.AppendLine(reader.GetString(0));
                }
            }

            MessageBox.Show(stringBuilder.ToString(), "Популярные жанры 2010-2011");
        }

        private void ShortFilmsWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new ShortFilms(FilmsDbConnection)
            {
                Owner = this
            };
            window.ShowDialog();
        }

        private void AsterixWithoutObelixWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();

            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand(
                    "Select title from films where title like '%Астерикс%' And not title like '%Обеликс%'",
                    FilmsDbConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return;

                    while (reader.Read())
                        stringBuilder.AppendLine(reader.GetString(0));
                }
            }

            MessageBox.Show(stringBuilder.ToString(), "Фильмы с Астериксом без Обеликса");
        }

        private void AlphaviteFilmsWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new AlphaviteFilms(new AlphaviteFilmsViewModel(FilmsDbConnection))
            {
                Owner = this
            };
            window.ShowDialog();
        }

        private void DeleteComedies_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите удалить все фильмы жанра 'комедия'?",
                "Удаление комедий",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            var count = DeleteComedies();
            MessageBox.Show($"Удалено комедий: {count}");
        }

        private void UpdateFilmsDuration_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите заменить пустые значения длительности фильмов на 42?",
                "Установка длительности",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            var count = UpdateDuration();
            MessageBox.Show($"Обновлено фильмов: {count}");
        }

        private void UpdateFantasticFilmsDurationButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите увеличить длительность фантастических фильмов вдвое?",
                "Установка длительности",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            var count = ElongateFantasticFilmsDuration();
            MessageBox.Show($"Обновлено фильмов: {count}");
        }

        private void DeleteReverseAlphaTitleButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы действительно хотите удалить фильмы, названия которых начинаются на 'Я' и заканчиваются на 'а'?",
                "Удаление фильмов",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            var count = DeleteReverseAlphaNameFilms();
            MessageBox.Show($"Удалено фильмов: {count}");
        }

        private void FilmsGeneratorWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = new FilmsGeneratorViewModel(FilmsDbConnection);
            var window = new FilmsGenerator
            {
                DataContext = vm,
                Owner = this
            };
            window.ShowDialog();
        }

        private void FilmsLibraryWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = new FilmsLibraryViewModel(FilmsDbConnection);
            var window = new FilmsLibrary
            {
                DataContext = vm,
                Owner = this
            };
            window.ShowDialog();
        }

        private void DeleteLongActionsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы действительно хотите удалить фильмы жанра 'боевик' и длительностью 90 минут и более?",
                "Удаление фильмов",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            var count = DeleteLongActions();
            MessageBox.Show($"Удалено фильмов: {count}");
        }

        private void ShortenMusicalsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите сократить длительность всех мюзиклов до 100?",
                "Установка длительности",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            var count = UpdateMusicalDuration();
            MessageBox.Show($"Обновлено фильмов: {count}");
        }

        private void ShortenOldFilmsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите укоротить длительность всех фильмов 1973 года втрое?",
                "Установка длительности",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            var count = UpdateOldFilmsDuration();
            MessageBox.Show($"Обновлено фильмов: {count}");
        }

        private void DeleteOldLongFantasticsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы действительно хотите удалить все фильмы жанра 'фантастика', вышедшие до 2000 года с длительностью более 90 минут?",
                "Удаление фильмов",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            var count = DeleteOldLongFantastics();
            MessageBox.Show($"Удалено фильмов: {count}");
        }

        #region Methods

        private int DeleteComedies()
        {
            var count = 0;
            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand(
                    "Delete from films where genre = (Select id from genres where title = 'комедия')",
                    FilmsDbConnection);
                count = command.ExecuteNonQuery();
            }

            return count;
        }

        private int DeleteLongActions()
        {
            var count = 0;
            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand(
                    "Delete from films where genre = (Select id from genres where title = 'боевик') AND duration >= 90",
                    FilmsDbConnection);
                count = command.ExecuteNonQuery();
            }

            return count;
        }

        private int DeleteOldLongFantastics()
        {
            var count = 0;
            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand(
                    "Delete from films where genre = (Select id from genres where title = 'фантастика') AND duration > 90 AND year < 2000",
                    FilmsDbConnection);
                count = command.ExecuteNonQuery();
            }

            return count;
        }

        private int UpdateDuration()
        {
            var count = 0;
            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand(
                    "Update films set duration = 42 where duration = ''",
                    FilmsDbConnection);
                count = command.ExecuteNonQuery();
            }

            return count;
        }

        private int UpdateMusicalDuration()
        {
            var count = 0;
            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand(
                    "Update films set duration = 100 where genre = (Select id from genres where title = 'мюзикл') AND duration > 100",
                    FilmsDbConnection);
                count = command.ExecuteNonQuery();
            }

            return count;
        }

        private int UpdateOldFilmsDuration()
        {
            var count = 0;
            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand(
                    "Update films set duration = duration/3 where year = 1973",
                    FilmsDbConnection);
                count = command.ExecuteNonQuery();
            }

            return count;
        }

        private int ElongateFantasticFilmsDuration()
        {
            var count = 0;
            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand(
                    "Update films set duration = duration*2 where genre = (Select id from genres where title = 'фантастика')",
                    FilmsDbConnection);
                count = command.ExecuteNonQuery();
            }

            return count;
        }

        private int DeleteReverseAlphaNameFilms()
        {
            var count = 0;
            using (FilmsDbConnection)
            {
                FilmsDbConnection.Open();
                var command = new SqliteCommand(
                    "Delete from films where title like 'Я%' and title like '%а'",
                    FilmsDbConnection);
                count = command.ExecuteNonQuery();
            }

            return count;
        }

        #endregion
    }
}