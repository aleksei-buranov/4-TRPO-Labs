using System.Windows;
using Microsoft.Data.Sqlite;

namespace Part78
{
    /// <summary>
    /// Interaction logic for LongComedies.xaml
    /// </summary>
    public partial class LongComedies : Window
    {
        public LongComedies(SqliteConnection sqliteConnection)
        {
            InitializeComponent();
            using (sqliteConnection)
            {
                sqliteConnection.Open();
                var command = new SqliteCommand("SELECT title From Films " +
                                                "Where duration > 60 AND genre = (" +
                                                "Select id From genres " +
                                                "Where title = 'комедия')",
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
