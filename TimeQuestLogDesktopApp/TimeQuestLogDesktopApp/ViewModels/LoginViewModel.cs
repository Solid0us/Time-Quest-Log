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
		private NavigationStore _navigationStore;

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

		public ICommand LoginCommand { get; set; }

		public LoginViewModel(NavigationStore navigationStore)
		{
			LoginCommand = new LoginCommand(this);
			_navigationStore = navigationStore;
		}

		public void NavigateTo(ViewModelBase viewModelBase)
		{
			_navigationStore.CurrentViewModel = viewModelBase;
		}
	}
}