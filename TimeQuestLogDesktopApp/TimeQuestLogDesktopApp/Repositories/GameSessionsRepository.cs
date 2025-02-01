using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.Models.DTOs;

namespace TimeQuestLogDesktopApp.Repositories
{
	internal class GameSessionsRepository : SqliteDataAccess
	{
		private readonly IDbConnectionFactory _connectionFactory;

        public GameSessionsRepository(SqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory; 
        }

        public List<GameSessionsDTO>GetGameSessionsByUserId(string userId)
        {
            List<GameSessionsDTO> gameSessionsDTOs = new List<GameSessionsDTO>();
            string sql = @"
                SELECT 
                    gameSession.Id,
                    gameSession.StartTime,
                    gameSession.EndTime,
                    game.Id,
                    game.Name,
                    game.CoverUrl
                FROM GameSessions gameSession
                INNER JOIN Games game ON game.Id = gameSession.GameId
                WHERE gameSession.UserId = @UserId
                ORDER BY gameSession.StartTime DESC
            ";
            var parameters = new { UserId = userId };
            using (var cnn = _connectionFactory.CreateConnection())
            {
                cnn.Query<GameSessions, Games, GameSessionsDTO>(
                    sql,
                    (gameSession, game) =>
                    {
                        GameSessionsDTO gameSessionsDTO = new GameSessionsDTO
                        {
                            Id = gameSession.Id,
                            StartTime = gameSession.StartTime,
                            EndTime = gameSession.EndTime,
                            SessionGame = new GameSessionsDTO.Game { Id = game.Id, Name = game.Name, CoverUrl = game.CoverUrl }
                        };
                        gameSessionsDTOs.Add(gameSessionsDTO);
                        return new GameSessionsDTO();
                    },
					 parameters,
					 splitOn: "Id"
					);
            }
            return gameSessionsDTOs;
        }

        public GameSessions CreateGameSession(int gameId, string userId)
        {
            string sql = @"
                INSERT INTO GameSessions
                (Id, StartTime, GameId, UserId)
                VALUES (@Id, @StartTime, @GameId, @UserId)
            ";
            string gameSessionId = Guid.NewGuid().ToString();
            DateTime currentUtcDateTime = DateTime.UtcNow;
            var parameters = new { Id = gameSessionId, StartTime = currentUtcDateTime, GameId = gameId, UserId = userId };
            using (var cnn = _connectionFactory.CreateConnection())
            {
                cnn.Execute(sql, parameters);
                return new GameSessions { Id = gameSessionId, StartTime = currentUtcDateTime, GameId = gameId, UserId = userId};
            }
        }

        public void EndGameSession(string gameSessionId)
        {
            string sql = @"
                UPDATE GameSessions
                SET Endtime = @EndTime
                WHERE Id = @GameSessionId
            ";
            var parameters = new { EndTime = DateTime.UtcNow, GameSessionId = gameSessionId };

            using (var cnn = _connectionFactory.CreateConnection())
            {
                cnn.Execute(sql, parameters);
            }

        }
    }
}
