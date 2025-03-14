using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models.API;
using TimeQuestLogDesktopApp.Models.DTOs;
using TimeQuestLogDesktopApp.Repositories;
using TimeQuestLogDesktopApp.Services;
using MessageBox = System.Windows.MessageBox;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.Commands
{
	internal class PullGamesFromCloudCommand : AsyncCommandBase
	{
		public readonly Action _loadGames;

		public PullGamesFromCloudCommand(Action loadGames)
		{
			_loadGames = loadGames;
		}
		protected async override Task ExecuteAsync(object? parameter)
		{
			SqliteConnectionFactory sqliteConnectionFactory = new SqliteConnectionFactory(new SqliteDataAccess().LoadConnectionString());
			UserGameRepository userGameRepository = new UserGameRepository(sqliteConnectionFactory);
			GenresRepository genresRepository = new GenresRepository(sqliteConnectionFactory);
			GamesRepository gameRepository = new GamesRepository(sqliteConnectionFactory);
			GameGenresRepository gameGenresRepository = new GameGenresRepository(sqliteConnectionFactory);

			HttpService _httpService = HttpService.GetInstance();
			string userId = CredentialManagerService.GetInstance().GetUserId(CredentialManagerService.CredentialType.REFRESH);
			string url = $"{EnvironmentVariableService.Instance.ApiBaseUrl}user-games/{userId}";
			HttpResponseMessage response = await _httpService.SendAndRepeatAuthorization(() => _httpService.GetAsync(url));

			if (response.IsSuccessStatusCode)
			{
				string message = await response.Content.ReadAsStringAsync();
				ApiResponse<List<UserGameAllDataDTO>> json = JsonConvert.DeserializeObject<ApiResponse<List<UserGameAllDataDTO>>>(message);
				foreach(UserGameAllDataDTO game in json.Data)
				{
					gameRepository.ReplaceGame(new Games { Id = game.Game.Id, Name = game.Game.Name, CoverUrl = game.Game.CoverUrl});
					genresRepository.CreateGenres(game.Game.Genres.ToList());
					gameGenresRepository.CreateGameGenres(game.Game.Id, game.Game.Genres.Select(item => item.Id).ToList());
					userGameRepository.ReplaceUserGame(new UserGames
					{
						Id = game.Id,
						ExeName = game.ExeName,
						GameId = game.Game.Id,
						UserId = userId
					});
				}
				_loadGames();
				GameSessionMonitoringService.Instance.LoadExeMap();
			}
			else if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				MessageBox.Show("Your session has expired. Please signout and log back in.", "Session Expired", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				MessageBox.Show("Something went wrong on the server side.", "Server Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}


    }
}
