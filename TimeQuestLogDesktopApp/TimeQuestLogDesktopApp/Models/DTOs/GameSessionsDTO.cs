using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models.DTOs
{
	internal class GameSessionsDTO
	{
		public string Id { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime? EndTime { get; set; }

		public class User
		{
			public string Id { get; set; } 
			public string Username { get; set; } 
			public string FirstName { get; set; } 
			public string LastName { get; set; } 
			public string Email { get; set; } 
		}

		public class Game
		{
			public int Id { get; set; } 
			public string Name { get; set; } 
			public List<string> Genres { get; set; } 
			public string CoverUrl { get; set; } 
		}

		public User SessionUser { get; set; }
		public Game SessionGame { get; set; } 

		public GameSessionsDTO()
		{
			SessionUser = new User();
			SessionGame = new Game();
		}

	}
}
