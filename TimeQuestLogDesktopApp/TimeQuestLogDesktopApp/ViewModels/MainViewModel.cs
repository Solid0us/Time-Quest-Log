using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.Stores;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
		private readonly NavigationStore _mainViewModelNavigationStore;
		public ViewModelBase CurrentViewModel => _mainViewModelNavigationStore.CurrentViewModel;

		public MainViewModel(NavigationStore mainViewModelNavigationStore)
		{
			_mainViewModelNavigationStore = mainViewModelNavigationStore;
			CredentialManagerService credentialManagerService = new CredentialManagerService();
			credentialManagerService.Load();
			if (credentialManagerService.GetUserId() != null && credentialManagerService.GetUsername != null)
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
