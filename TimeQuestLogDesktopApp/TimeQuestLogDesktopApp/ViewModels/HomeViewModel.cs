using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models.DTOs;
using TimeQuestLogDesktopApp.Repositories;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.Stores;

namespace TimeQuestLogDesktopApp.ViewModels
{
	internal class HomeViewModel : ViewModelBase
	{
		private List<GameSessionsDTO> _gameSessionTable = new List<GameSessionsDTO>();
		private GameSessionsRepository _gameSessionsRepository;
		private readonly CredentialManagerService _credentialManagerService;

		public List<GameSessionsDTO> GameSessionTable
		{
			get { return _gameSessionTable; }
			set 
			{ 
				_gameSessionTable = value; 
				OnPropertyChanged(nameof(GameSessionTable));
			}
		}


		public HomeViewModel()
        {
			_credentialManagerService = CredentialManagerService.GetInstance();
			_credentialManagerService.LoadCredentials();
			LoadGameSessions();
        }

		public void LoadGameSessions()
		{
			var sqliteDataAccess = new SqliteDataAccess();
			var sqliteConnectionFactory = new SqliteConnectionFactory(sqliteDataAccess.LoadConnectionString());
			_gameSessionsRepository = new GameSessionsRepository(sqliteConnectionFactory);
			List<GameSessionsDTO> gameSessionsDTOs = _gameSessionsRepository.GetGameSessionsByUserId(_credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH));

			_gameSessionTable.Clear();
			foreach (var gameSession in gameSessionsDTOs)
			{
				_gameSessionTable.Add(gameSession);
			}
		}
    }
}
