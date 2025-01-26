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
    class GenresRepository : SqliteDataAccess
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenresRepository(SqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void CreateGenres(List<Genres> genres)
        {
            
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT OR IGNORE INTO Genres (Id, Name) VALUES (@Id, @Name)";

                        command.Parameters.Add(new SQLiteParameter("@Id"));
                        command.Parameters.Add(new SQLiteParameter("@Name"));

                        foreach (var genre in genres)
                        {
							((SQLiteParameter)command.Parameters["@Id"]).Value = genre.Id;
							((SQLiteParameter)command.Parameters["@Name"]).Value = genre.Name;

							command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
        }
    }
}
