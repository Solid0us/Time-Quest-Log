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

namespace TimeQuestLogDesktopApp.Commands
{
	internal class LoginCommand : AsyncCommandBase
	{
		private readonly LoginViewModel _loginViewModel;
		private readonly NavigationStore _navigationStore;	
		private EnvironmentVariableService EnvironmentVariableService;
		private readonly HttpService _httpService = HttpService.GetInstance();
		private readonly SqliteDataAccess _sqliteDataAccess;
		private readonly CredentialManagerService _credentialManagerService;

        public LoginCommand(LoginViewModel loginViewModel, NavigationStore navigationStore)
        {
            _loginViewModel = loginViewModel;
			_sqliteDataAccess = new SqliteDataAccess();
			_navigationStore = navigationStore;
			EnvironmentVariableService = new EnvironmentVariableService();
			_credentialManagerService = CredentialManagerService.GetInstance();
        }
        protected override async Task ExecuteAsync(object? parameter)
		{
			try
			{
				string url = $"{EnvironmentVariableService.ApiBaseUrl}users/login";
				HttpResponseMessage response = await _httpService.PostAsync(url, _loginViewModel);

				string message = await response.Content.ReadAsStringAsync();
				AuthResponse json = JsonConvert.DeserializeObject<AuthResponse>(message);
				if (response.IsSuccessStatusCode)
				{
					MessageBox.Show($"You have successfully logged in! Here is your token: {json?.Token}");
					_credentialManagerService.SetUsername(CredentialManagerService.CredentialType.REFRESH,json?.UserId, json?.Username);
					_credentialManagerService.SetPassword(CredentialManagerService.CredentialType.REFRESH,json?.RefreshToken);
					_credentialManagerService.Save(CredentialManagerService.CredentialType.REFRESH);

					_credentialManagerService.SetUsername(CredentialManagerService.CredentialType.JWT, json?.UserId, json?.Username);
					_credentialManagerService.SetPassword(CredentialManagerService.CredentialType.JWT, json?.Token);
					_credentialManagerService.Save(CredentialManagerService.CredentialType.JWT);

					SqliteConnectionFactory sqliteConnectionFactory = new SqliteConnectionFactory(_sqliteDataAccess.LoadConnectionString());
					UserRepository userRepository = new UserRepository(sqliteConnectionFactory);
					Users? existingUser = userRepository.GetUserById(json.UserId);
					if (existingUser == null)
					{
						userRepository.SaveUsers(new Users(json.UserId, json.Username));
					}

					_navigationStore.CurrentViewModel = new DashboardViewModel(_navigationStore);
				}
				else if (response.StatusCode == HttpStatusCode.Unauthorized)
				{
					MessageBox.Show($"{json?.Error}");
				}
				else
				{
					MessageBox.Show($"Something went wrong.");
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
