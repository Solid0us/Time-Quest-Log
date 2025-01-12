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
		private readonly NavigationStore _navigationStore;
		public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

		public MainViewModel(NavigationStore navigationStore)
		{
			_navigationStore = navigationStore;
			CredentialManagerService credentialManagerService = new CredentialManagerService();
			credentialManagerService.Load();
			if (credentialManagerService.GetUserId() != null && credentialManagerService.GetUsername != null)
			{
				_navigationStore.CurrentViewModel = new DashboardViewModel();
			}
			else
			{
				_navigationStore.CurrentViewModel = CreateLoginViewModel();
			}
			_navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

		}

		private void OnCurrentViewModelChanged()
		{
			OnPropertyChanged(nameof(CurrentViewModel));
		}

		private LoginViewModel CreateLoginViewModel()
		{
			return new LoginViewModel(_navigationStore);
		}

	}
}
