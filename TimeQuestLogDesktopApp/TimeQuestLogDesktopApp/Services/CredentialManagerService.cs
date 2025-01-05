using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeQuestLogDesktopApp.Services
{
	internal class CredentialManagerService
	{
		private readonly Credential _credential;

		public CredentialManagerService(string target = "timequestlog-app", PersistanceType persistanceType = PersistanceType.LocalComputer)
		{
			_credential = new Credential
			{
				Target = target,
				PersistanceType = persistanceType
			};
		}

		public void SetUsername(string username)
		{
			_credential.Username = username;
		}

		public void SetPassword(string password)
		{
			_credential.Password = password;
		}

		public void Save()
		{
			_credential.Save();
		}

		public Credential Load()
		{
			_credential.Load();
			return _credential;
		}

		public void Delete()
		{
			_credential.Delete();
		}

		public Credential GetCredential() => _credential;
	}
}
