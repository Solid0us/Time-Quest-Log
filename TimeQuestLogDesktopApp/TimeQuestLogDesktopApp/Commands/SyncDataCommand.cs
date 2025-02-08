using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models.API;
using TimeQuestLogDesktopApp.Models.DTOs;
using TimeQuestLogDesktopApp.Repositories;
using TimeQuestLogDesktopApp.Services;

namespace TimeQuestLogDesktopApp.Commands
{
	internal class SyncDataCommand : AsyncCommandBase
	{
		private UserGameRepository _userGameRepository;
		private GameSessionsRepository _gameSessionsRepository;
		private EnvironmentVariableService _environmentVariableService;
		private CredentialManagerService _credentialManagerService;
		private HttpService _httpService;
		private const int BatchSize = 100;
		public SyncDataCommand() 
		{
			var sqliteDataAccess = new SqliteDataAccess();
			var sqliteConnectionFactory = new SqliteConnectionFactory(sqliteDataAccess.LoadConnectionString());
			_userGameRepository = new UserGameRepository(sqliteConnectionFactory);
			_gameSessionsRepository = new GameSessionsRepository(sqliteConnectionFactory);
			_environmentVariableService = EnvironmentVariableService.Instance;
			_credentialManagerService = CredentialManagerService.GetInstance();
			_httpService = HttpService.GetInstance();
		}
		protected async override Task ExecuteAsync(object? parameter)
		{
			string userId = _credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH);
			var unsyncedGameSessions = _gameSessionsRepository.GetUnsyncedGameSessionsByUserId(userId);
			bool bRet = await SyncUserGames();
			bRet = await SyncUserGameSessions();
			
		}

		private async Task<bool> SyncUserGames()
		{
			string userId = _credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH);
			var unsyncedUserGames = _userGameRepository.GetUnsyncedUserGamesByUserId(userId);
			string userGameUrl = _environmentVariableService.ApiBaseUrl + $"user-games/sync/{userId}";
			List<dynamic> data = new List<dynamic>();
			foreach (var batch in unsyncedUserGames.Chunk(BatchSize))
			{
				var batchList = batch.ToList();
				foreach (var item in batchList)
				{
					data.Add(new
					{
						id = item.Id,
						user = new
						{
							id = userId
						},
						game = new
						{
							id = item.GameId,
							name = item.Title,
							coverUrl = item.Cover
						},
						exeName = item.Exe
					});
				}
			}
			HttpResponseMessage response = await _httpService.SendAndRepeatAuthorization(() => _httpService.PutAsync(userGameUrl, data));
			string message = await response.Content.ReadAsStringAsync();
			ApiResponse<List<UserGameDTO>> json = JsonConvert.DeserializeObject<ApiResponse<List<UserGameDTO>>>(message);
			foreach (var userGameDTO in json.Data)
			{
				_userGameRepository.UpdateSync(userGameDTO.Id, true);
			}
			return true;
		}

		private async Task<bool> SyncUserGameSessions()
		{
			string userId = _credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH);
			var unsyncedGameSessions = _gameSessionsRepository.GetUnsyncedGameSessionsByUserId(userId);
			string userGameUrl = _environmentVariableService.ApiBaseUrl + $"game-sessions/sync/{userId}";
			List<dynamic> data = new List<dynamic>();
			foreach (var batch in unsyncedGameSessions.Chunk(BatchSize))
			{
				var batchList = batch.ToList();
				foreach (var item in batchList)
				{
					data.Add(new
					{
						id = item.Id,
						user = new
						{
							id = userId
						},
						game = new
						{
							id = item.GameId
						},
						StartTime = item.StartTime,
						EndTime = item.EndTime,
					});
				}
			}
			HttpResponseMessage response = await _httpService.SendAndRepeatAuthorization(() => _httpService.PutAsync(userGameUrl, data));
			string message = await response.Content.ReadAsStringAsync();
			ApiResponse<List<UserGameDTO>> json = JsonConvert.DeserializeObject<ApiResponse<List<UserGameDTO>>>(message);
			foreach (var userGameDTO in json.Data)
			{
				_gameSessionsRepository.UpdateSync(userGameDTO.Id, true);
			}
			return true;
		}

	}
}
