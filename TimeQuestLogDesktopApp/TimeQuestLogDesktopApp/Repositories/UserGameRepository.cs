using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.Models.DTOs;

namespace TimeQuestLogDesktopApp.Repositories
{
    internal class UserGameRepository : SqliteDataAccess
	{
		private readonly IDbConnectionFactory _connectionFactory;

        public UserGameRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory; 
        }

        public List<UserGameDTO> GetUserGames(string userId)
        {
            List<UserGameDTO> userGames = new List<UserGameDTO>();
			using (var cnn = _connectionFactory.CreateConnection())
			{
				string sql = @"
                SELECT 
					game.id,
                    game.Name, 
                    game.CoverUrl,
                    u.Username,
                    ug.ExeName,
                    gen.Id,
                    gen.Name
                FROM UserGames ug
                INNER JOIN Users u ON u.Id = ug.UserId
                INNER JOIN Games game ON game.Id = ug.GameId
                LEFT JOIN GameGenre gg ON game.Id = gg.GameId
                LEFT JOIN Genres gen ON gg.GenreId = gen.Id
                WHERE u.Id = @UserId
                ORDER BY game.Name";
				var gamesDict = new Dictionary<int, UserGameDTO>();

				cnn.Query<Games, Users, UserGames, Genres, UserGameDTO>(
					sql,
					(game, user, userGame, genre) =>
					{
						if (!gamesDict.TryGetValue(game.Id, out UserGameDTO existingGame))
						{
							UserGameDTO newUserGame = new UserGameDTO(game.Id, game.Name, game.CoverUrl, userGame.ExeName, user.Username);

							gamesDict.Add(game.Id, newUserGame);
							existingGame = newUserGame;
						}

						if (genre != null && !existingGame.Genres.Any(g => g.Id == genre.Id))
						{
							existingGame.Genres.Add(genre);
						}

						return existingGame;
					},
					param: new { UserId = userId },
					splitOn: "Username,ExeName,Id"
				);

				return gamesDict.Values.ToList();
			}
        }

		public int CreateUserGame(UserGames userGame)
		{
			using(var cnn = _connectionFactory.CreateConnection())
			{
				string sql = @"
					INSERT OR IGNORE INTO UserGames
						(Id, ExeName, GameId, UserId)
					VALUES	
						(@Id, @ExeName, @GameId, @UserId)
				";
				var parameters = new { userGame.Id, userGame.ExeName, userGame.GameId, userGame.UserId };

				return cnn.Execute(sql, parameters);
			}
		}

		public int UpdateSync(string id, bool isSynced)
		{
			using(var cnn = _connectionFactory.CreateConnection())
			{
				int syncValue = isSynced ? 1 : 0;
				string sql = @"
					UPDATE UserGames
					SET IsSynced = @SyncValue
					WHERE Id = @Id
				";
				var parameters = new { Id = id, SyncValue = syncValue };

				return cnn.Execute(sql, parameters);
			}
		}

		public IEnumerable<UserGameDTO> GetUnsyncedUserGamesByUserId(string userId)
		{
			using(var cnn = _connectionFactory.CreateConnection())
			{
				string sql = @"
                SELECT 
					game.id,
                    game.Name, 
                    game.CoverUrl,
                    u.Username,
					ug.Id,
                    ug.ExeName,
                    gen.Id,
                    gen.Name
                FROM UserGames ug
                INNER JOIN Users u ON u.Id = ug.UserId
                INNER JOIN Games game ON game.Id = ug.GameId
                LEFT JOIN GameGenre gg ON game.Id = gg.GameId
                LEFT JOIN Genres gen ON gg.GenreId = gen.Id
                WHERE u.Id = @UserId
				AND ug.IsSynced = 0";
				var gamesDict = new Dictionary<int, UserGameDTO>();

				cnn.Query<Games, Users, UserGames, Genres, UserGameDTO>(
					sql,
					(game, user, userGame, genre) =>
					{
						if (!gamesDict.TryGetValue(game.Id, out UserGameDTO existingGame))
						{
							UserGameDTO newUserGame = new UserGameDTO(userGame.Id, game.Id, game.Name, game.CoverUrl, userGame.ExeName, user.Username);

							gamesDict.Add(game.Id, newUserGame);
							existingGame = newUserGame;
						}

						if (genre != null && !existingGame.Genres.Any(g => g.Id == genre.Id))
						{
							existingGame.Genres.Add(genre);
						}

						return existingGame;
					},
					param: new { UserId = userId },
					splitOn: "Username,Id,Id"
				);

				return gamesDict.Values.ToList();
			}
		}
    }
}
