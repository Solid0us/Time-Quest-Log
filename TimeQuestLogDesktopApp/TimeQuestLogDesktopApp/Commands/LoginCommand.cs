using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.Commands
{
	internal class LoginCommand : AsyncCommandBase
	{
		private readonly LoginViewModel _loginViewModel;
		private EnvironmentVariableService EnvironmentVariableService;
		private readonly HttpService _httpService;

        public LoginCommand(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
			_httpService = new HttpService();
        }
        protected override async Task ExecuteAsync(object? parameter)
		{
			EnvironmentVariableService = new EnvironmentVariableService();
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
					credentialManager.SetUsername(json?.Username);
					credentialManager.SetPassword(_loginViewModel.Password);
					credentialManager.Save();
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
