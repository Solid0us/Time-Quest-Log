using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models
{
	internal class GameSessions
	{
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int GameId { get; set; }
        public string UserId { get; set; }

        public GameSessions()
        {
            Id = string.Empty;
            StartTime = DateTime.UtcNow;
            EndTime = null;
            GameId = 0;
            UserId = string.Empty;
        }

        public GameSessions(int gameId, string userId)
        {
            Id = Guid.NewGuid().ToString();
            StartTime = DateTime.UtcNow;
            EndTime = null;
            GameId = gameId;
            UserId = userId;
        }
    }
}
