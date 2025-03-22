using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.Repositories;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.Stores;
using TimeQuestLogDesktopApp.ViewModels;
using MessageBox = System.Windows.MessageBox;

namespace TimeQuestLogDesktopApp.Commands
{
	internal class LoginCommand : AsyncCommandBase
	{
		private readonly LoginViewModel _loginViewModel;
		private readonly NavigationStore _navigationStore;	
		private EnvironmentVariableService _environmentVariableService;
		private readonly HttpService _httpService = HttpService.GetInstance();
		private readonly SqliteDataAccess _sqliteDataAccess;
		private readonly CredentialManagerService _credentialManagerService;

        public LoginCommand(LoginViewModel loginViewModel, NavigationStore navigationStore)
        {
            _loginViewModel = loginViewModel;
			_sqliteDataAccess = new SqliteDataAccess();
			_navigationStore = navigationStore;
			_environmentVariableService = EnvironmentVariableService.Instance;
			_credentialManagerService = CredentialManagerService.GetInstance();
        }
        protected override async Task ExecuteAsync(object? parameter)
		{
			try
			{
				bool loginSuccess = await LoginService.Instance.Login(_loginViewModel.Username, _loginViewModel.Password);
				if (loginSuccess)
				{
					_navigationStore.CurrentViewModel = new DashboardViewModel(_navigationStore);
					GameSessionMonitoringService.Instance.LoadGameSessions();
					GameSessionMonitoringService.Instance.LoadExeMap();
				}
			}
			catch (HttpRequestException ex)
			{
				MessageBox.Show($"Network error: {ex.Message}");
			}
			catch (JsonException ex)
			{
				MessageBox.Show($"Error processing the response: {ex.Message}");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An unexpected error occurred: {ex.Message}");
			}

		}
	}
}
