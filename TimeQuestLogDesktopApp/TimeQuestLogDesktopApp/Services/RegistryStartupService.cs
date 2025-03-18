using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Services
{
	internal class RegistryStartupService
	{
		private readonly string _appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{AppDomain.CurrentDomain.FriendlyName}.exe");
		private static RegistryStartupService _instance;
		private static readonly object lockObject = new object();

		private RegistryStartupService()
        {
            
        }

		public static RegistryStartupService Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (lockObject)
					{
						if (_instance == null)
						{
							_instance = new RegistryStartupService();
						}
					}
				}
				return _instance;
			}
		}

        public bool IsStartupEnabled(string appName)
		{
			using (var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false))
			{
				return key?.GetValue(appName) != null;
			}
		}

		public void EnableStartup(string appName)
		{
			using (var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
			{
				key.SetValue(appName, _appPath);
			}
		}

		public void DisableStartup(string appName)
		{
			using (var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
			{
				key.DeleteValue(appName, false);
			}
		}

	}
}
