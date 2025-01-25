
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeQuestLogDesktopApp.Commands;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.Stores;

namespace TimeQuestLogDesktopApp.ViewModels
{
	internal class DashboardViewModel : ViewModelBase
	{
		private readonly NavigationStore _dashboardNavigationStore;
		public ViewModelBase CurrentDashboardViewModel => _dashboardNavigationStore.CurrentViewModel;
		private LibraryViewModel _libraryViewModel;
		public ICommand SignoutCommand { get; set; }
		public ICommand NavigateToHome { get; set; }
		public ICommand NavigateToLibrary { get; private set; }
		public ICommand NavigateToSettings { get; set; }
		public string Username { get; private set; }
		private readonly CredentialManagerService _credentialManager;

		public DashboardViewModel(NavigationStore mainViewModelNavigationStore)
		{
			_credentialManager = CredentialManagerService.GetInstance();
			_credentialManager.LoadCredentials();
			Username = _credentialManager.GetUsername(CredentialManagerService.CredentialType.REFRESH);
			SignoutCommand = new SignoutCommand(mainViewModelNavigationStore);

			_dashboardNavigationStore = new NavigationStore();
			_dashboardNavigationStore.CurrentViewModel = new HomeViewModel();
			_dashboardNavigationStore.CurrentViewModelChanged += OnCurrentDashboardViewModelChanged;

			NavigateToHome = new NavigateCommand<HomeViewModel>(_dashboardNavigationStore, () => new HomeViewModel());
			NavigateToSettings = new NavigateCommand<SettingsViewModel>(_dashboardNavigationStore, () => new SettingsViewModel());

			InitializeLibraryNavigationAsync();
		}

		private async void InitializeLibraryNavigationAsync()
		{
			_libraryViewModel = await LibraryViewModel.CreateAsync();
			NavigateToLibrary = new NavigateCommand<LibraryViewModel>(_dashboardNavigationStore, () => _libraryViewModel);
			OnPropertyChanged(nameof(NavigateToLibrary));
		}

		private void OnCurrentDashboardViewModelChanged()
		{
			OnPropertyChanged(nameof(CurrentDashboardViewModel));
		}
	}
}
