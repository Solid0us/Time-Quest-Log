using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;

namespace TimeQuestLogDesktopApp.Repositories
{
    class GamesRepository : SqliteDataAccess
    {
		private readonly IDbConnectionFactory _connectionFactory;
        public GamesRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Games? GetGameById(int id)
        {
			string sql = @"SELECT * FROM Games WHERE Id = @Id";
            var parameters = new {Id  = id};
			using (var cnn = _connectionFactory.CreateConnection())
            {
                return cnn.QuerySingleOrDefault<Games>(sql, parameters);
            }
        }

        public int CreateGame(Games game)
        {
            string sql = @"INSERT OR IGNORE INTO Games (Id, Name, CoverUrl) VALUES (@Id, @Name, @CoverUrl)";
            var parameters = new { game.Id, game.Name, game.CoverUrl };

            using (var cnn = _connectionFactory.CreateConnection())
            {
                return cnn.Execute(sql, parameters);
            }
        }

        public int ReplaceGame(Games game)
        {
			string sql = @"REPLACE INTO Games (Id, Name, CoverUrl, IsSynced) VALUES (@Id, @Name, @CoverUrl, @IsSynced)";
			var parameters = new { game.Id, game.Name, game.CoverUrl, IsSynced = true };

			using (var cnn = _connectionFactory.CreateConnection())
			{
				return cnn.Execute(sql, parameters);
			}
		}
    }
}
