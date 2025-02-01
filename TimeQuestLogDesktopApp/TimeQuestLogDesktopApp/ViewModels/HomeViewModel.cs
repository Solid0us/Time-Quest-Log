using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
		private ObservableCollection<GameSessionsDTO> _gameSessionTable;
		private GameSessionsRepository _gameSessionsRepository;
		private readonly CredentialManagerService _credentialManagerService;
		private GameSessionMonitoringService _gameSessionMonitoringService;

		public ObservableCollection<GameSessionsDTO> GameSessionTable
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
			_gameSessionMonitoringService = GameSessionMonitoringService.GetInstance;
			_gameSessionTable = new ObservableCollection<GameSessionsDTO>(_gameSessionMonitoringService.GameSessions);
			_gameSessionMonitoringService.GameSessionsChanged += OnGameSessionsChanged;
		}

		private void OnGameSessionsChanged(object sender, List<GameSessionsDTO> sessions)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				GameSessionTable = new ObservableCollection<GameSessionsDTO>(sessions);
			});
		}

		public override void Dispose()
		{
			_gameSessionMonitoringService.GameSessionsChanged -= OnGameSessionsChanged;
			base.Dispose();
		}
	}
}
