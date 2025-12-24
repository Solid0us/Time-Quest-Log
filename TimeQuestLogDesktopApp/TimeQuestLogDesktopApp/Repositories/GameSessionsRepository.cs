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

        public List<GameSessionsDTO> GetGameSessionsByUserId(string userId)
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
                LIMIT 50
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

        public GameSessions CreateGameSession(GameSessions gameSession)
        {
            string sql = @"
                INSERT INTO GameSessions
                (Id, StartTime, GameId, UserId)
                VALUES (@Id, @StartTime, @GameId, @UserId)
            ";
            var parameters = new { gameSession.Id, gameSession.StartTime, gameSession.GameId, gameSession.UserId };
            using (var cnn = _connectionFactory.CreateConnection())
            {
                cnn.Execute(sql, parameters);
                return new GameSessions
                {
                    Id = gameSession.Id,
                    StartTime = gameSession.StartTime,
                    GameId = gameSession.GameId,
                    UserId = gameSession.UserId
                };
            }
        }

        public void UpdateGameSessionEndTime(GameSessions gameSession)
        {
            string sql = @"
                UPDATE GameSessions
                SET Endtime = @EndTime,
                IsSynced = @IsSynced
                WHERE Id = @GameSessionId
            ";
            var parameters = new { gameSession.EndTime, GameSessionId = gameSession.Id, IsSynced = false};

            using (var cnn = _connectionFactory.CreateConnection())
            {
                cnn.Execute(sql, parameters);
            }
        }

        public int UpdateSync(string id, bool isSynced)
        {
            using (var cnn = _connectionFactory.CreateConnection())
            {
                int syncValue = isSynced ? 1 : 0;
                string sql = @"
					UPDATE GameSessions
					SET IsSynced = @SyncValue
					WHERE Id = @Id
				";
                var parameters = new { Id = id, SyncValue = syncValue };

                return cnn.Execute(sql, parameters);
            }
        }

        public bool IsUserGameInProgress(string userId, int gameId)
        {
            using (var cnn = _connectionFactory.CreateConnection())
            {
                string sql = @"
                    SELECT COUNT(1) 
                    FROM GameSessions
                    WHERE GameId = @GameId
                    AND UserId = @UserId
                    AND EndTime IS NULL
                ";
                var parameters = new { GameId = gameId, UserId = userId };
                return cnn.ExecuteScalar<int>(sql, parameters) > 0;
            }
        }

        public IEnumerable<GameSessions> GetUnsyncedGameSessionsByUserId(string userId)
        {
            using (var cnn = _connectionFactory.CreateConnection())
            {
                string sql = @"
                    SELECT * FROM GameSessions
                    WHERE UserId = @UserId
                    AND IsSynced = 0
                ";
                var parameters = new { UserId = userId };
                return cnn.Query<GameSessions>(sql, parameters);
            }
        }
    }
}
