using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.Commands
{
	internal class LoginCommand : CommandBase
	{
		private LoginViewModel _loginViewModel;
		private EnvironmentVariableService EnvironmentVariableService;

        public LoginCommand(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
        }
        public override void Execute(object? parameter)
		{
			EnvironmentVariableService = new EnvironmentVariableService();
			MessageBoxResult result = MessageBox.Show($"Username: {_loginViewModel.Username}\nPassword: {_loginViewModel.Password}");
		}
	}
}
