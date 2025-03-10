using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.Repositories;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.Stores;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
		private readonly NavigationStore _mainViewModelNavigationStore;
		private readonly SqliteDataAccess _sqliteDataAccess;
		public ViewModelBase CurrentViewModel => _mainViewModelNavigationStore.CurrentViewModel;

		public MainViewModel(NavigationStore mainViewModelNavigationStore)
		{
			_sqliteDataAccess = new SqliteDataAccess();
			_mainViewModelNavigationStore = mainViewModelNavigationStore;
			CredentialManagerService credentialManagerService = CredentialManagerService.GetInstance();
			credentialManagerService.LoadCredentials();
			string? userId = credentialManagerService.GetUserId(CredentialManagerService.CredentialType.REFRESH);
			string? username = credentialManagerService.GetUsername(CredentialManagerService.CredentialType.REFRESH);
			SqliteConnectionFactory sqliteConnectionFactory = new SqliteConnectionFactory(_sqliteDataAccess.LoadConnectionString());
			UserRepository userRepository = new UserRepository(sqliteConnectionFactory);
			Users? existingUser = userRepository.GetUserById(userId);
			if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(username) && existingUser != null)
			{
				_mainViewModelNavigationStore.CurrentViewModel = new DashboardViewModel(_mainViewModelNavigationStore);
			}
			else
			{
				_mainViewModelNavigationStore.CurrentViewModel = CreateLoginViewModel();
			}
			_mainViewModelNavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

		}

		private void OnCurrentViewModelChanged()
		{
			OnPropertyChanged(nameof(CurrentViewModel));
		}

		private LoginViewModel CreateLoginViewModel()
		{
			return new LoginViewModel(_mainViewModelNavigationStore);
		}

	}
}
