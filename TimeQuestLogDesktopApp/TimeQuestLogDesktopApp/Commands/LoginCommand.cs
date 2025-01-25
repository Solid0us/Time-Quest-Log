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
		private readonly HttpService _httpService;
		private readonly SqliteDataAccess _sqliteDataAccess;

        public LoginCommand(LoginViewModel loginViewModel, NavigationStore navigationStore)
        {
            _loginViewModel = loginViewModel;
			_httpService = new HttpService();
			_sqliteDataAccess = new SqliteDataAccess();
			_navigationStore = navigationStore;
			EnvironmentVariableService = new EnvironmentVariableService();
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
					var credentialManager = new CredentialManagerService();
					credentialManager.SetUsername(json?.UserId, json?.Username);
					credentialManager.SetPassword(json?.RefreshToken);
					credentialManager.Save();

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
