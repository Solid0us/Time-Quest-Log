
using CredentialManagement;
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
        CredentialManagerService CredentialManager { get; set; }
        public ICommand SignoutCommand { get; set; }
        public ICommand NavigateToHome {  get; set; }
        public ICommand NavigateToLibrary { get; set; }
        public ICommand NavigateToSettings {  get; set; }
        public string Username { get; set; }

        public DashboardViewModel(NavigationStore mainViewModelNavigationStore)
        {
            CredentialManager = new CredentialManagerService();
            CredentialManager.Load();
            Username = CredentialManager.GetUsername();
            SignoutCommand = new SignoutCommand(mainViewModelNavigationStore);

            _dashboardNavigationStore = new NavigationStore();
            _dashboardNavigationStore.CurrentViewModel = new HomeViewModel();
            _dashboardNavigationStore.CurrentViewModelChanged += OnCurrentDashboardViewModelChanged;

            NavigateToHome = new NavigateCommand<HomeViewModel>(_dashboardNavigationStore, () => new HomeViewModel());
            NavigateToLibrary = new NavigateCommand<LibraryViewModel>(_dashboardNavigationStore, () => new LibraryViewModel());
            NavigateToSettings = new NavigateCommand<SettingsViewModel>(_dashboardNavigationStore, () => new SettingsViewModel());
		}

		private void OnCurrentDashboardViewModelChanged()
		{
			OnPropertyChanged(nameof(CurrentDashboardViewModel));
		}
	}
}
