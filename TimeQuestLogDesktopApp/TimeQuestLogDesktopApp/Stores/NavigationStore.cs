using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.Stores
{
	internal class NavigationStore
	{
		public ViewModelBase _currentViewModel;
		public ViewModelBase CurrentViewModel
		{
			get => _currentViewModel;
			set
			{
				_currentViewModel = value;
				OnCurrentViewModelChanged();
			}
		}

		public async Task NavigateAsync(Func<Task<ViewModelBase>> createViewModelAsync)
		{
			var viewModel = await createViewModelAsync();
			CurrentViewModel = viewModel;
		}

		private void OnCurrentViewModelChanged()
		{
			CurrentViewModelChanged?.Invoke();
		}

		public event Action CurrentViewModelChanged;
	}
}
