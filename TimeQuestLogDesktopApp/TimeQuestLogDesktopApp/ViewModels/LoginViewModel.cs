using System.Windows.Input;
using TimeQuestLogDesktopApp.Commands;
using TimeQuestLogDesktopApp.Stores;

namespace TimeQuestLogDesktopApp.ViewModels
{
	internal class LoginViewModel : ViewModelBase
	{
		private string _username;

		public string Username
		{
			get { return _username; }
			set 
			{ 
				_username = value; 
				OnPropertyChanged(nameof(Username));
			}
		}

		private string _password;

		public string Password
		{
			get { return _password; }
			set
			{ 
				_password = value; 
				OnPropertyChanged(nameof(Password));
			}
		}

		public ICommand LoginCommand { get; set; }

		public LoginViewModel()
		{
			LoginCommand = new LoginCommand(this);
		}
	}
}