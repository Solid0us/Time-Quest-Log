using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeQuestLogDesktopApp.Commands;
using TimeQuestLogDesktopApp.Stores;

namespace TimeQuestLogDesktopApp.ViewModels
{
	[JsonObject(MemberSerialization.OptIn)]
	internal class SignupViewModel : ViewModelBase
	{
		private readonly NavigationStore _navigationStore;
        private string _username;
        public ICommand NavigateToLogin { get; set; }
        public ICommand SignupCommand { get; set; }

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

        private string _firstName;

        [JsonProperty("firstName")]
        public string FirstName
        {
            get { return _firstName; }
            set 
            { 
                _firstName = value; 
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;

        [JsonProperty("lastName")]
        public string LastName
        {
            get { return _lastName; }
            set 
            { 
                _lastName = value; 
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _email;

		[JsonProperty("email")]
		public string Email
        {
            get { return _email; }
            set 
            {
                _email = value; 
                OnPropertyChanged(nameof(Email));
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

        private string _confirmPassword;

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set 
            { 
                _confirmPassword = value; 
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }


        public SignupViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            Password = string.Empty;
            Username = string.Empty;
            Email = string.Empty;
            ConfirmPassword = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;

            NavigateToLogin = new NavigateCommand<LoginViewModel>(navigationStore, () => new LoginViewModel(navigationStore));
            SignupCommand = new SignupCommand(this, navigationStore);
        }
    }
}
