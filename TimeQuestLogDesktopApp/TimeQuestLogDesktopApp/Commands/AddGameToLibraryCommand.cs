using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.Repositories;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.Commands
{
	class AddGameToLibraryCommand : AsyncCommandBase
	{
		private readonly AddGameViewModel _addGameViewModel;
		private readonly CredentialManagerService _credentialManagerService;
		private readonly SqliteDataAccess _sqliteDataAccess;
		protected async override Task ExecuteAsync(object? parameter)
		{
			// TODO: Add Validation 
			SqliteConnectionFactory sqliteConnectionFactory = new SqliteConnectionFactory(_sqliteDataAccess.LoadConnectionString());
			UserGameRepository userGameRepository = new UserGameRepository(sqliteConnectionFactory);
			GamesRepository gameRepository = new GamesRepository(sqliteConnectionFactory);
			GenresRepository genresRepository = new GenresRepository(sqliteConnectionFactory);
			GameGenresRepository gameGenresRepository = new GameGenresRepository(sqliteConnectionFactory);
			UserGames userGame = new UserGames
			{
				Id = Guid.NewGuid().ToString(),
				ExeName = _addGameViewModel.ExeName,
				GameId = _addGameViewModel.SelectedGame.id,
				UserId = _credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH)
			};

			Games game = new Games
			{
				Id = _addGameViewModel.SelectedGame.id,
				Name = _addGameViewModel.SelectedGame.name,
				CoverUrl = _addGameViewModel.SelectedGame.cover?.url ?? ""
			};
			Games existingGame = gameRepository.GetGameById(game.Id);
			if (existingGame == null)
			{
				gameRepository.CreateGame(game);
				List<Genres> genres = new List<Genres>();
				foreach (var genre in _addGameViewModel.SelectedGame.genres)
				{
					genres.Add(new Genres{ Id = genre.id, Name = genre.name});
				}
				genresRepository.CreateGenres(genres);
				gameGenresRepository.CreateGameGenres(game.Id, genres.Select(item => item.Id).ToList());
			}
			userGameRepository.CreateUserGame(userGame);
			// Add to server
			HttpService httpService = HttpService.GetInstance();
			try
			{
				HttpResponseMessage response = await httpService.SendAndRepeatAuthorization(() => httpService.PutAsync(EnvironmentVariableService.Instance.ApiBaseUrl + $"user-games/{userGame.Id}",
					new
					{
						user = new {id = userGame.UserId},
						game = new {id = userGame.GameId},
						exeName = userGame.ExeName
					}));
				if (response.IsSuccessStatusCode)
				{
					userGameRepository.UpdateSync(userGame.Id, true);
				}
				else
				{
					userGameRepository.UpdateSync(userGame.Id, false);
				}
				_addGameViewModel._loadGames();
				GameSessionMonitoringService.Instance.MapGame(_addGameViewModel.ExeName, _addGameViewModel.SelectedGame.id);
				MessageBox.Show($"{game.Name} has been successfully added to your library!", "Game Added", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "HTTP Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

        public AddGameToLibraryCommand(AddGameViewModel addGameViewModel)
        {
            _addGameViewModel = addGameViewModel;
			_credentialManagerService = CredentialManagerService.GetInstance();
			_sqliteDataAccess = new SqliteDataAccess();
        }
    }
}
