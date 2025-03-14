using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models.DTOs
{
	internal class UserGameAllDataDTO
	{
		public string Id;
		public GameApi Game;
		public string ExeName;

		public class GameApi
		{
			public int Id;
			public string Name;
			public Genres[] Genres;
			public string CoverUrl;
		}

		public UserGameAllDataDTO(string id, GameApi game, string exeName)
        {
            Id = id;
			Game = game;
			ExeName = exeName;
        }
    }
}
