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

namespace TimeQuestLogDesktopApp.Commands
{
	class SignupCommand : AsyncCommandBase
	{
		private readonly SignupViewModel _signupViewModel;
		private readonly NavigationStore _navigationStore;
		private readonly HttpService _httpService;
		private EnvironmentVariableService EnvironmentVariableService;
		private readonly SqliteDataAccess _sqliteDataAccess;
		public SignupCommand(SignupViewModel signupViewModel, NavigationStore navigationStore) 
		{
			_signupViewModel = signupViewModel;
			_navigationStore = navigationStore;
			_httpService = new HttpService();
			_sqliteDataAccess = new SqliteDataAccess();
			EnvironmentVariableService = new EnvironmentVariableService();	
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
					string url = $"{EnvironmentVariableService.ApiBaseUrl}users/register";
					HttpResponseMessage response = await _httpService.PostAsync(url, _signupViewModel);

					string message = await response.Content.ReadAsStringAsync();
					AuthResponse json = JsonConvert.DeserializeObject<AuthResponse>(message);
					if (response.IsSuccessStatusCode)
					{
						MessageBox.Show($"Hi {_signupViewModel.Username}, you have registered!", "Successful User Registration", MessageBoxButton.OK, MessageBoxImage.Information);
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
