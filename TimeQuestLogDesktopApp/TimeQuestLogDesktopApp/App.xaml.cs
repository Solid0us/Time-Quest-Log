using System.Configuration;
using System.Data;
using System.Windows;
using TimeQuestLogDesktopApp.Services;
using TimeQuestLogDesktopApp.Stores;
using TimeQuestLogDesktopApp.ViewModel;

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
