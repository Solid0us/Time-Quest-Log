using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.Stores;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.Commands
{
	class SignoutCommand : CommandBase
	{
		private readonly NavigationStore _mainViewModelNavigationStore;
		public override void Execute(object? parameter)
		{
			MessageBoxResult result = MessageBox.Show("Are you sure you want to sign out? You will need internet access to log back in.",
											 "Log Out",
											 MessageBoxButton.YesNo,
											 MessageBoxImage.Information);
			if (result == MessageBoxResult.Yes)
			{
				CredentialManagerService credentialManagerService = CredentialManagerService.GetInstance();
				credentialManagerService.LoadCredentials();
				credentialManagerService.Delete(CredentialManagerService.CredentialType.REFRESH);
				credentialManagerService.Delete(CredentialManagerService.CredentialType.JWT);
				_mainViewModelNavigationStore.CurrentViewModel = new LoginViewModel(_mainViewModelNavigationStore);
				GameSessionMonitoringService.Instance.ClearMapsAndSessions();
			}
		}

        public SignoutCommand(NavigationStore mainViewModelNavigationStore)
        {
			_mainViewModelNavigationStore = mainViewModelNavigationStore;
        }
    }
}
