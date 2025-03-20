using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeQuestLogDesktopApp.Services;

namespace TimeQuestLogDesktopApp.Commands
{
	internal class ToggleStartupCommand : CommandBase
	{
		private string _appName;
		private readonly ScheduledTaskService _startupService;
		public override void Execute(object? parameter)
		{
			ToggleStartup();
		}

        public ToggleStartupCommand(string appName)
        {
            _appName = appName;
			_startupService = ScheduledTaskService.Instance;
        }

		private void ToggleStartup()
		{
			if (!_startupService.IsStartupEnabled())
			{
				_startupService.EnableStartup();
			}
			else
			{
				_startupService.DisableStartup();
			}
		}
	}
}
