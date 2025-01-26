using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;

namespace TimeQuestLogDesktopApp.Repositories
{
    class GameGenresRepository : SqliteDataAccess
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GameGenresRepository(SqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void CreateGameGenres(int gameId, List<int> genreIds)
        {
			using (var connection = _connectionFactory.CreateConnection())
			{
				connection.Open();
				using (var transaction = connection.BeginTransaction())
				{
					using (var command = connection.CreateCommand())
					{
						command.CommandText = "INSERT OR IGNORE INTO GameGenre (GameId, GenreId) VALUES (@GameId, @GenreId)";

						command.Parameters.Add(new SQLiteParameter("@GameId"));
						command.Parameters.Add(new SQLiteParameter("@GenreId"));

						foreach (var genre in genreIds)
						{
							((SQLiteParameter)command.Parameters["@GameId"]).Value = gameId;
							((SQLiteParameter)command.Parameters["@GenreId"]).Value = genre;

							command.ExecuteNonQuery();
						}
					}

					transaction.Commit();
				}
			}
		}
    }
}
