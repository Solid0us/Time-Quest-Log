using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeQuestLogDesktopApp.Commands;
using TimeQuestLogDesktopApp.Models.DTOs;

namespace TimeQuestLogDesktopApp.ViewModels
{
	internal class AddGameViewModel : ViewModelBase
	{
		private string _gameSearch;
		private List<IGDBGame> _games;

		public string GameSearch
		{
			get => _gameSearch;
			set 
			{ 
				_gameSearch = value; 
				OnPropertyChanged(nameof(GameSearch));
			}
		}

		public List<IGDBGame> Games
		{
			get => _games;
			set
			{
				_games = value;
				OnPropertyChanged(nameof(Games));
			}
		}


		public ICommand SearchIGDBGames { get; set; }

        public AddGameViewModel()
        {
            GameSearch = string.Empty;
			Games = new List<IGDBGame>();
            SearchIGDBGames = new SearchIGDBGamesCommand(this);
        }
    }
}
