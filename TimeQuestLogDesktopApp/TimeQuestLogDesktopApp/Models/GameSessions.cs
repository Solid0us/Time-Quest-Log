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
            StartTime = DateTime.Now;
            EndTime = null;
            GameId = 0;
            UserId = string.Empty;
        }
    }
}
