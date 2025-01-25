using System.Collections.ObjectModel;
using System.Windows.Input;
using TimeQuestLogDesktopApp;
using TimeQuestLogDesktopApp.Commands;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models.DTOs;
using TimeQuestLogDesktopApp.Repositories;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.ViewModels
{
    internal class LibraryViewModel : ViewModelBase
	{
		private ObservableCollection<UserGameDTO> _userGameTable;
		private readonly UserGameRepository _userGameRepository;
		private readonly CredentialManagerService _credentialManagerService;

		public ICommand ShowAddGameWindow {  get; set; }

		public ObservableCollection<UserGameDTO> UserGamesTable
		{
			get => _userGameTable;
			set
			{
				_userGameTable = value;
				OnPropertyChanged(nameof(UserGamesTable));
			}
		}

		public LibraryViewModel()
		{
			var sqliteDataAccess = new SqliteDataAccess();
			var sqliteConnectionFactory = new SqliteConnectionFactory(sqliteDataAccess.LoadConnectionString());
			_userGameRepository = new UserGameRepository(sqliteConnectionFactory);
			_credentialManagerService = CredentialManagerService.GetCredentialManagerService();
			_credentialManagerService.LoadCredentials();
			UserGamesTable = new ObservableCollection<UserGameDTO>();
			ShowAddGameWindow = new ShowAddGameCommand();
		}

		public static async Task<LibraryViewModel> CreateAsync()
		{
			var vm = new LibraryViewModel();
			await vm.InitializeAsync();
			return vm;
		}

		public async Task InitializeAsync()
		{
			var userId = _credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH);
			var games = await Task.Run(() => _userGameRepository.GetUserGames(userId));

			App.Current.Dispatcher.Invoke(() =>
			{
				UserGamesTable.Clear();
				foreach (var game in games)
				{
					UserGamesTable.Add(game);
				}
			});
		}

		public async Task RefreshDataAsync()
		{
			await InitializeAsync();
		}
	}
}