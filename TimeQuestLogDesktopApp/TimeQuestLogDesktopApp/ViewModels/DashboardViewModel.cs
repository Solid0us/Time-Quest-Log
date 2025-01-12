
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
        CredentialManagerService CredentialManager { get; set; }
        public ICommand SignoutCommand { get; set; }
        public string Username { get; set; }

        public DashboardViewModel(NavigationStore mainViewModelNavigationStore)
        {
            CredentialManager = new CredentialManagerService();
            CredentialManager.Load();
            Username = CredentialManager.GetUsername();
            SignoutCommand = new SignoutCommand(mainViewModelNavigationStore);
        }
    }
}
