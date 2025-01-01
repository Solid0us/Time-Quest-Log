using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Stores;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.Views
{
    internal class MainViewModel : ViewModelBase
    {
		private readonly NavigationStore _navigationStore;
		public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

		public MainViewModel(NavigationStore navigationStore)
		{
			_navigationStore = navigationStore;
			_navigationStore.CurrentViewModel = CreateLoginViewModel();
			_navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

		}

		private void OnCurrentViewModelChanged()
		{
			OnPropertyChanged(nameof(CurrentViewModel));
		}

		private LoginViewModel CreateLoginViewModel()
		{
			return new LoginViewModel();
		}

	}
}
