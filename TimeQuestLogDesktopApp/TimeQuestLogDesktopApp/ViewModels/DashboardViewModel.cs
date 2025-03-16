
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeQuestLogDesktopApp.Commands;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Repositories;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.Stores;

namespace TimeQuestLogDesktopApp.ViewModels
{
	internal class DashboardViewModel : ViewModelBase
	{
		private readonly NavigationStore _dashboardNavigationStore;
		public ViewModelBase CurrentDashboardViewModel => _dashboardNavigationStore.CurrentViewModel;
		private string _currentDashboardViewModelName = "Home";

		public string CurrentDashboardViewModelName
		{
			get { return _currentDashboardViewModelName; }
			set 
			{ 
				_currentDashboardViewModelName = value;
				OnPropertyChanged(nameof(CurrentDashboardViewModelName));
			}
		}


		private LibraryViewModel _libraryViewModel;

		public int NumberUnsynced
		{
			get { return _gameMonitoringService.NumberUnsynced; }
			set
			{
				_gameMonitoringService.NumberUnsynced = value;
				OnPropertyChanged(nameof(NumberUnsynced));
			}
		}

		public bool IsSyncButtonEnabled => NumberUnsynced != 0;

		public ICommand SignoutCommand { get; set; }
		public ICommand NavigateToHome { get; set; }
		public ICommand NavigateToLibrary { get; private set; }
		public ICommand NavigateToSettings { get; set; }
		public ICommand SyncData { get; set; }
		[DllImport("kernel32.dll")]
		private static extern bool AllocConsole();
		public string Username { get; private set; }
		private readonly CredentialManagerService _credentialManager;
		private readonly GameSessionMonitoringService _gameMonitoringService = GameSessionMonitoringService.Instance;

		public DashboardViewModel(NavigationStore mainViewModelNavigationStore)
		{
			_gameMonitoringService.PropertyChanged += GameMonitoringService_PropertyChanged;
			if (System.Diagnostics.Debugger.IsAttached)
			{
				AllocConsole();
			}
			using (_gameMonitoringService)
			{
				_gameMonitoringService.StartMonitoring();
			}
			_gameMonitoringService.StartMonitoring();
			_credentialManager = CredentialManagerService.GetInstance();
			_credentialManager.LoadCredentials();
			Username = _credentialManager.GetUsername(CredentialManagerService.CredentialType.REFRESH);
			Username = UpperCaseString(Username);
			SignoutCommand = new SignoutCommand(mainViewModelNavigationStore);

			_dashboardNavigationStore = new NavigationStore();
			_dashboardNavigationStore.CurrentViewModel = new HomeViewModel();
			_dashboardNavigationStore.CurrentViewModelChanged += OnCurrentDashboardViewModelChanged;

			NavigateToHome = new NavigateCommand<HomeViewModel>(_dashboardNavigationStore, () =>
			{
				CurrentDashboardViewModelName = "Home";
				return new HomeViewModel();
			});
			NavigateToSettings = new NavigateCommand<SettingsViewModel>(_dashboardNavigationStore, () =>
			{
				CurrentDashboardViewModelName = "Settings";
				return new SettingsViewModel();
			});

			SyncData = new SyncDataCommand();
			UpdateUnsyncedCounter();

			InitializeLibraryNavigationAsync();
		}

		private static string UpperCaseString(string val)
		{
			if (string.IsNullOrEmpty(val)) return val;
			return char.ToUpper(val[0]) + val.Substring(1);
		}

		private void GameMonitoringService_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(GameSessionMonitoringService.NumberUnsynced))
			{
				OnPropertyChanged(nameof(NumberUnsynced));
				OnPropertyChanged(nameof(IsSyncButtonEnabled));
			}
		}

		private async void InitializeLibraryNavigationAsync()
		{
			_libraryViewModel = await LibraryViewModel.CreateAsync();
			NavigateToLibrary = new NavigateCommand<LibraryViewModel>(_dashboardNavigationStore, () =>
			{
				CurrentDashboardViewModelName = "Library";
				return _libraryViewModel;
			});
			OnPropertyChanged(nameof(NavigateToLibrary));
		}

		private void OnCurrentDashboardViewModelChanged()
		{
			OnPropertyChanged(nameof(CurrentDashboardViewModel));
			OnPropertyChanged(nameof(CurrentDashboardViewModelName));
		}

		public void UpdateUnsyncedCounter()
		{
			var sqliteDataAccess = new SqliteDataAccess();
			var sqliteConnectionFactory = new SqliteConnectionFactory(sqliteDataAccess.LoadConnectionString());
			string userId = _credentialManager.GetUserId(CredentialManagerService.CredentialType.REFRESH);
			GameSessionsRepository gameSessionRepository = new GameSessionsRepository(sqliteConnectionFactory);
			UserGameRepository userGameRepository = new UserGameRepository(sqliteConnectionFactory);
			NumberUnsynced = gameSessionRepository.GetUnsyncedGameSessionsByUserId(userId).Count()
								+ userGameRepository.GetUnsyncedUserGamesByUserId(userId).Count();
		}

	
	}
}
