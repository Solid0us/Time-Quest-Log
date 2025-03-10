using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models.DTOs
{
	internal class UserGameSyncDTO
	{
        public string Id { get; set; }
        public UserObj User { get; set; }

		public GameObj Game { get; set; }

        internal class UserObj
		{
			public string Id;
		}

		internal class GameObj
		{
			public int Id;
		}

        public UserGameSyncDTO(string id, UserObj user, GameObj game)
        {
            Id = id;
			User = user;
			Game = game;
        }
    }
}
