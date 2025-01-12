using System.Configuration;
using System.Data;
using System.Windows;
using TimeQuestLogDesktopApp.Stores;
using TimeQuestLogDesktopApp.ViewModel;

namespace TimeQuestLogDesktopApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly NavigationStore _navigationStore;
		public App()
		{
			_navigationStore = new NavigationStore();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			MainWindow = new MainWindow()
			{
				DataContext = new MainViewModel(_navigationStore)
			};
			MainWindow.Show();
			base.OnStartup(e);
		}

	}

}
