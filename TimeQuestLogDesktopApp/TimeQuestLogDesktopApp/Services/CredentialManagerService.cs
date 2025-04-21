using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeQuestLogDesktopApp.Services
{
	public class CredentialManagerService
	{
		private static readonly CredentialManagerService _instance = new CredentialManagerService();
		public enum CredentialType
		{
			REFRESH,
			JWT,
			LOGIN
		}
		private readonly Credential _refreshTokenCredential;
		private readonly Credential _jwtCredential;
		private readonly Credential _loginCredential;

		private CredentialManagerService()
		{
			_refreshTokenCredential = new Credential
			{
				Target = "timequestlog-app-Refresh",
				PersistanceType = PersistanceType.LocalComputer
			};

			_jwtCredential = new Credential
			{
				Target = "timequestlog-app-JWT",
				PersistanceType = PersistanceType.LocalComputer
			};
			_loginCredential = new Credential
			{
				Target = "timequestlog-app-Login",
				PersistanceType = PersistanceType.LocalComputer
			};
			LoadCredentials();
		}

		public static CredentialManagerService GetInstance()
		{
			return _instance;
		}

		public string GetPassword(CredentialType type)
		{
			if (type == CredentialType.REFRESH)
			{
				return _refreshTokenCredential.Password;
			}
			else if (type == CredentialType.JWT)
			{
				return _jwtCredential.Password;
			}
			else
			{
				return _loginCredential.Password;
			}
		}

		public void SetUsername(CredentialType type, string userId,  string username)                 
		{
			if (type == CredentialType.REFRESH)
			{
				_refreshTokenCredential.Username = userId + ";" + username;   
			}
			else
			{
				_jwtCredential.Username = userId + ";" + username;
			}
		}

		public void SetUsername(string username)
		{
			_loginCredential.Username = username;
		}

		public void SetPassword(CredentialType type, string password)
		{
			if (type == CredentialType.REFRESH)
			{
				_refreshTokenCredential.Password = password;
			}
			else if (type == CredentialType.JWT)
			{
				_jwtCredential.Password = password;
			}
			else
			{
				_loginCredential.Password = password;
			}
		}

		public void Save(CredentialType type)
		{
			if (type == CredentialType.REFRESH)
			{
				_refreshTokenCredential.Save();
			}
			else if (type == CredentialType.JWT)
			{
				_jwtCredential.Save();
			}
			else
			{
				_loginCredential.Save();
			}
		}

		public void LoadCredentials()
		{
			_refreshTokenCredential.Load();
			_jwtCredential.Load();
			_loginCredential.Load();
		}

		public void Delete(CredentialType type)
		{
			if (type == CredentialType.REFRESH)
			{
				_refreshTokenCredential.Delete();
			}
			else if (type == CredentialType.JWT)
			{
				_jwtCredential.Delete();
			}
			else
			{
				_loginCredential.Delete();
			}
		}

		public string GetUsername(CredentialType type)
		{
			string username = String.Empty;
			if (type == CredentialType.REFRESH)
			{
				if (_refreshTokenCredential.Username != null)
				{
					username = _refreshTokenCredential.Username.Split(';')[1];
				}
			}
			else if (type == CredentialType.JWT)
			{
				if (_jwtCredential.Username != null)
				{
					username = _jwtCredential.Username.Split(";")[1];
				}
			}
			else
			{
				username = _loginCredential.Username;
			}
			return username;
		}

		public string GetUserId(CredentialType type)
		{
			string userId = String.Empty;
			if (type == CredentialType.REFRESH)
			{
				if (_refreshTokenCredential.Username != null)
				{
					userId = _refreshTokenCredential.Username.Split(';')[0];
				}
			}
			else
			{
				if (_jwtCredential.Username != null)
				{
					userId = _jwtCredential.Username.Split(",")[0];
				}
			}
			return userId;
		}
	}
}
