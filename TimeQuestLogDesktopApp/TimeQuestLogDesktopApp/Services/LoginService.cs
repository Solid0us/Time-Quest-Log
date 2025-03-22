using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Database;
using TimeQuestLogDesktopApp.Models;
using TimeQuestLogDesktopApp.Repositories;
using TimeQuestLogDesktopApp.Stores;
using TimeQuestLogDesktopApp.ViewModels;

namespace TimeQuestLogDesktopApp.Services
{
	internal class LoginService
	{
		private readonly CredentialManagerService _credentialManagerService;
		private EnvironmentVariableService _environmentVariableService;
		private static LoginService _loginService = new LoginService();
		private readonly HttpService _httpService = HttpService.GetInstance();
		private readonly SqliteDataAccess _sqliteDataAccess;

		private LoginService()
        {
			_credentialManagerService = CredentialManagerService.GetInstance();
			_environmentVariableService = EnvironmentVariableService.Instance;
			_sqliteDataAccess = new SqliteDataAccess();
		}

		public static LoginService Instance
		{
			get
			{
				return _loginService;
			}
		}

		public async Task<bool> Login(string username, string password)
		{
			bool bRet = false;
			string url = $"{_environmentVariableService.ApiBaseUrl}users/login";
			HttpResponseMessage response = await _httpService.PostAsync(url, 
			new
			{
				username, password
			});
			string message = await response.Content.ReadAsStringAsync();
			AuthResponse json = JsonConvert.DeserializeObject<AuthResponse>(message);
			if (response.IsSuccessStatusCode)
			{
				_credentialManagerService.SetUsername(CredentialManagerService.CredentialType.REFRESH, json?.UserId, json?.Username);
				_credentialManagerService.SetPassword(CredentialManagerService.CredentialType.REFRESH, json?.RefreshToken);
				_credentialManagerService.Save(CredentialManagerService.CredentialType.REFRESH);

				_credentialManagerService.SetUsername(CredentialManagerService.CredentialType.JWT, json?.UserId, json?.Username);
				_credentialManagerService.SetPassword(CredentialManagerService.CredentialType.JWT, json?.Token);
				_credentialManagerService.Save(CredentialManagerService.CredentialType.JWT);

				_credentialManagerService.SetUsername(json?.Username);
				_credentialManagerService.SetPassword(CredentialManagerService.CredentialType.LOGIN, password);
				_credentialManagerService.Save(CredentialManagerService.CredentialType.LOGIN);

				SqliteConnectionFactory sqliteConnectionFactory = new SqliteConnectionFactory(_sqliteDataAccess.LoadConnectionString());
				UserRepository userRepository = new UserRepository(sqliteConnectionFactory);
				Users? existingUser = userRepository.GetUserById(json.UserId);
				if (existingUser == null)
				{
					userRepository.SaveUsers(new Users(json.UserId, json.Username));
				}
				bRet = true;
			}
			else if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				MessageBox.Show($"{json?.Error}");
			}
			else
			{
				MessageBox.Show($"Something went wrong.");
			}

			return bRet;
		}
	}
}
