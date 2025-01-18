using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models
{
	internal class GameGenre
	{
        public int GameId { get; set; }
        public int GenreId { get; set; }

        public GameGenre(int gameId, int genreId)
        {
            GameId = gameId;
            GenreId = genreId;
        }
    }
}
