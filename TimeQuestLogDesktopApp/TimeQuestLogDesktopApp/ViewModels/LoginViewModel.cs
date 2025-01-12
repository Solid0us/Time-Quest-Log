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
		private readonly NavigationStore _mainViewModelNavigationStore;
		public ICommand LoginCommand { get; set; }
		public ICommand NavigateToSignup { get; set; }


		public LoginViewModel(NavigationStore mainViewModelNavigationStore)
		{
			LoginCommand = new LoginCommand(this, mainViewModelNavigationStore);
			_mainViewModelNavigationStore = mainViewModelNavigationStore;
			NavigateToSignup = new NavigateCommand<SignupViewModel>(
				_mainViewModelNavigationStore,
				() => new SignupViewModel(_mainViewModelNavigationStore)
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