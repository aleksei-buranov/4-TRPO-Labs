using System.Windows;
using Microsoft.Data.Sqlite;

namespace Part78
{
    /// <summary>
    /// Interaction logic for ShortFilms.xaml
    /// </summary>
    public partial class ShortFilms : Window
    {
        public ShortFilms(SqliteConnection sqliteConnection)
        {
            InitializeComponent();
            using (sqliteConnection)
            {
                sqliteConnection.Open();
                var command = new SqliteCommand("Select title from films where duration < 86",
                    sqliteConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return;
                    while (reader.Read())
                        FilmsList.Items.Add(reader.GetString(0));
                }
            }
        }
    }
}
