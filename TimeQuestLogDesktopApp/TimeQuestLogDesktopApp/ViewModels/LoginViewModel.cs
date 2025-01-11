using Newtonsoft.Json;
using System.Windows.Input;
using TimeQuestLogDesktopApp.Commands;
using TimeQuestLogDesktopApp.Stores;

namespace TimeQuestLogDesktopApp.ViewModels
{
	[JsonObject(MemberSerialization.OptIn)]
	internal class LoginViewModel : ViewModelBase
	{
		private string _username;
		private readonly NavigationStore _navigationStore;
		public ICommand LoginCommand { get; set; }
		public ICommand NavigateToSignup { get; set; }


		public LoginViewModel(NavigationStore navigationStore)
		{
			LoginCommand = new LoginCommand(this, navigationStore);
			_navigationStore = navigationStore;
			NavigateToSignup = new NavigateCommand<SignupViewModel>(
				_navigationStore,
				() => new SignupViewModel(_navigationStore)
			);
		}

		[JsonProperty("username")]
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

		[JsonProperty("password")]
		public string Password
		{
			get { return _password; }
			set
			{ 
				_password = value; 
				OnPropertyChanged(nameof(Password));
			}
		}
	}
}