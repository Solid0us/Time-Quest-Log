using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.Stores;
using TimeQuestLogDesktopApp.ViewModel;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace TimeQuestLogDesktopApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly NavigationStore _mainViewModelNavigationStore;
		public App()
		{
			Process currentProcess = Process.GetCurrentProcess();
			Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
			if (processes.Length >  1)
			{
				Shutdown();
				MessageBox.Show("An instance of this application is already running.", "Warning", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Warning);
				return;
			}
			_mainViewModelNavigationStore = new NavigationStore();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			MainWindow = new MainWindow()
			{
				DataContext = new MainViewModel(_mainViewModelNavigationStore)
			};
			MainWindow.Show();
			base.OnStartup(e);
			Current.Exit += OnApplicationExit;
		}

		private void OnApplicationExit(object sender, ExitEventArgs e)
		{
			GameSessionMonitoringService.Instance.StopMonitoring();
			GameSessionMonitoringService.Instance.Dispose();
		}

	}

}
