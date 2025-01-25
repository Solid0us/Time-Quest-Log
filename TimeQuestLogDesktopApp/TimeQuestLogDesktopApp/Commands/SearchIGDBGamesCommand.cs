using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.Models.API;
using TimeQuestLogDesktopApp.Models.DTOs;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.Commands
{
	internal class SearchIGDBGamesCommand : AsyncCommandBase
	{
        private readonly AddGameViewModel _gameViewModel;
		private readonly HttpService _httpService;
		private readonly SqliteDataAccess _sqliteDataAccess;
		private EnvironmentVariableService EnvironmentVariableService;
		public SearchIGDBGamesCommand(AddGameViewModel gameViewModel)
		{
            _gameViewModel = gameViewModel;
			_httpService = new HttpService();
			_sqliteDataAccess = new SqliteDataAccess();
			EnvironmentVariableService = new EnvironmentVariableService();
		}
        protected override async Task ExecuteAsync(object? parameter)
		{
			string url = $"{EnvironmentVariableService.ApiBaseUrl}games?name={_gameViewModel.GameSearch}";
			HttpResponseMessage response = await _httpService.GetAsync(url);

			string message = await response.Content.ReadAsStringAsync();
			ApiResponse <List<IGDBGame>> json = JsonConvert.DeserializeObject<ApiResponse<List<IGDBGame>>>(message);
			foreach (IGDBGame game in json.Data)
			{
				if (game.cover != null)
				{
					game.cover.url = "https:" + game.cover.url;
				}
			}
			_gameViewModel.Games = new List<IGDBGame>(json.Data);
		}
	}
}
