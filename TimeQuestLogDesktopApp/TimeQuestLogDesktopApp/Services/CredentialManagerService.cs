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
			JWT
		}
		private readonly Credential _refreshTokenCredential;
		private readonly Credential _jwtCredential;

		private CredentialManagerService()
		{
			_refreshTokenCredential = new Credential
			{
				Target = "timequestlog-app",
				PersistanceType = PersistanceType.LocalComputer
			};

			_jwtCredential = new Credential
			{
				Target = "timequestlog-app-JWT",
				PersistanceType = PersistanceType.LocalComputer
			};
		}

		public static CredentialManagerService GetCredentialManagerService()
		{
			return _instance;
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

		public void SetPassword(CredentialType type, string password)
		{
			if (type == CredentialType.REFRESH)
			{
				_refreshTokenCredential.Password = password;
			}
			else
			{
				_jwtCredential.Password = password;
			}
		}

		public void Save(CredentialType type)
		{
			if (type == CredentialType.REFRESH)
			{
				_refreshTokenCredential.Save();
			}
			else
			{
				_jwtCredential.Save();
			}
		}

		public void LoadCredentials()
		{
			_refreshTokenCredential.Load();
			_jwtCredential.Load();
		}

		public void Delete(CredentialType type)
		{
			if (type == CredentialType.REFRESH)
			{
				_refreshTokenCredential.Delete();
			}
			else
			{
				_jwtCredential.Delete();
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
			else
			{
				if (_jwtCredential.Username != null)
				{
					username = _jwtCredential.Username.Split(";")[1];
				}
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
