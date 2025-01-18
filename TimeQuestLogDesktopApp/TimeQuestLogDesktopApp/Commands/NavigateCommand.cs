using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeQuestLogDesktopApp.Stores;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.Commands
{
	internal class NavigateCommand<TViewModel> : ICommand where TViewModel : ViewModelBase
	{
		private readonly NavigationStore _navigationStore;
		private readonly Func<ViewModelBase> _createViewModel;

		public NavigateCommand(NavigationStore navigationStore, Func<ViewModelBase> createViewModel)
		{
			_navigationStore = navigationStore;
			_createViewModel = createViewModel;
		}

		public event EventHandler? CanExecuteChanged;

		public bool CanExecute(object? parameter) => true;

		public void Execute(object? parameter)
		{
			var newViewModel = _createViewModel();
			if (_navigationStore.CurrentViewModel?.GetType() != newViewModel?.GetType())
			{
				_navigationStore.CurrentViewModel = newViewModel;
			}
		}
	}
}
