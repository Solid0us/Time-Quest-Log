using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TimeQuestLogDesktopApp.Commands;
using TimeQuestLogDesktopApp.Services;

namespace TimeQuestLogDesktopApp.ViewModels
{
	internal class SettingsViewModel : ViewModelBase
	{
		private readonly RegistryStartupService _startupService;
		private bool _isStartupEnabled;
		private const string AppName = "TimeQuestLog";
		public ICommand ToggleStartupCommand {  get; set; }

		public bool IsStartupEnabled
		{
			get => _isStartupEnabled;
			set
			{
				if (_isStartupEnabled != value)
				{
					_isStartupEnabled = value;
					OnPropertyChanged(nameof(IsStartupEnabled));
				}
			}
		}

		public SettingsViewModel()
		{
			ToggleStartupCommand = new ToggleStartupCommand(AppName);
			_startupService = RegistryStartupService.Instance;
			LoadStartupState();
		}


		private void LoadStartupState()
		{
			IsStartupEnabled = _startupService.IsStartupEnabled(AppName);
		}

	}
}
