
using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Services;

namespace TimeQuestLogDesktopApp.ViewModels
{
	internal class DashboardViewModel : ViewModelBase
	{
        CredentialManagerService CredentialManager { get; set; }
        public string Username { get; set; }

        public DashboardViewModel()
        {
            CredentialManager = new CredentialManagerService();
            CredentialManager.Load();
            Username = CredentialManager.GetUsername();
        }
    }
}
