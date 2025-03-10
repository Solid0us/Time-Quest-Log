using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.Repositories;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.Stores;
using TimeQuestLogDesktopApp.Utilities;
using TimeQuestLogDesktopApp.ViewModels;
using MessageBox = System.Windows.MessageBox;

namespace TimeQuestLogDesktopApp.Commands
{
	class SignupCommand : AsyncCommandBase
	{
		private readonly SignupViewModel _signupViewModel;
		private readonly NavigationStore _navigationStore;
		private readonly HttpService _httpService = HttpService.GetInstance();
		private EnvironmentVariableService _environmentVariableService;
		private readonly SqliteDataAccess _sqliteDataAccess;
		private readonly CredentialManagerService _credentialManagerService;
		public SignupCommand(SignupViewModel signupViewModel, NavigationStore navigationStore) 
		{
			_signupViewModel = signupViewModel;
			_navigationStore = navigationStore;
			_sqliteDataAccess = new SqliteDataAccess();
			_environmentVariableService = EnvironmentVariableService.Instance;	
			_credentialManagerService = CredentialManagerService.GetInstance();
		}

		public bool IsValidUserRegistration()
		{
			int errors = 0;
			ValidationUtils validationUtils = new ValidationUtils();
			var (password, confirmPassword, username, email) = (
				_signupViewModel.Password,
				_signupViewModel.ConfirmPassword,
				_signupViewModel.Username,
				_signupViewModel.Email
			);

			if (password.Length < 8) errors++;
			if (username.Length < 6) errors++;
			if (!ValidationUtils.IsValidEmail(email)) errors++;
			if (!password.Equals(confirmPassword)) errors++;


			if (errors > 0) return false;
			return true;
		}

		protected override async Task ExecuteAsync(object? parameter)
		{
			bool isValidRegistration = IsValidUserRegistration();
			if (isValidRegistration)
			{
				try
				{
					string url = $"{_environmentVariableService.ApiBaseUrl}users/register";
					HttpResponseMessage response = await _httpService.PostAsync(url, _signupViewModel);

					string message = await response.Content.ReadAsStringAsync();
					AuthResponse json = JsonConvert.DeserializeObject<AuthResponse>(message);
					if (response.IsSuccessStatusCode)
					{
						MessageBox.Show($"Hi {_signupViewModel.Username}, you have registered!", "Successful User Registration", MessageBoxButton.OK, MessageBoxImage.Information);
						_credentialManagerService.SetUsername(CredentialManagerService.CredentialType.REFRESH, json?.UserId, json?.Username);
						_credentialManagerService.SetPassword(CredentialManagerService.CredentialType.REFRESH, json?.RefreshToken);
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
						GameSessionMonitoringService.Instance.LoadGameSessions();
						GameSessionMonitoringService.Instance.LoadExeMap();
					}
					else
					{
						MessageBox.Show($"Could not register with server error: {json.Error}", "Invalid User Registration", MessageBoxButton.OK, MessageBoxImage.Error);
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
			else
			{
				MessageBox.Show("Please check the errors on the form", "Invalid User Registration", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
