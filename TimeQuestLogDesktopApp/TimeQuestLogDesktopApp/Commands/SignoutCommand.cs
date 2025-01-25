using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			CredentialManagerService credentialManagerService = CredentialManagerService.GetInstance();
			credentialManagerService.LoadCredentials();
			credentialManagerService.Delete(CredentialManagerService.CredentialType.REFRESH);
			credentialManagerService.Delete(CredentialManagerService.CredentialType.JWT);
			_mainViewModelNavigationStore.CurrentViewModel = new LoginViewModel(_mainViewModelNavigationStore);
		}

        public SignoutCommand(NavigationStore mainViewModelNavigationStore)
        {
			_mainViewModelNavigationStore = mainViewModelNavigationStore;
        }
    }
}
