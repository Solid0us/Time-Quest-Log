using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using Application = System.Windows.Application;
using System.ComponentModel;

namespace TimeQuestLogDesktopApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private NotifyIcon notifyIcon;
		private bool isExiting = false;
		public MainWindow()
		{
			InitializeComponent();
			if (Debugger.IsAttached)
			{
				Title += " (Debug ON)";
			}
			InitializeNotifyIcon();
		}

		private void InitializeNotifyIcon()
		{
			notifyIcon = new NotifyIcon();
			notifyIcon.Icon = new Icon("gamepad-logo.ico");
			notifyIcon.Text = "My App";
			notifyIcon.Visible = true;

			notifyIcon.DoubleClick += (s, args) => RestoreWindow();

			var contextMenu = new ContextMenuStrip();
			contextMenu.Items.Add("Open", null, (s, args) => RestoreWindow());
			contextMenu.Items.Add("Exit", null, (s, args) => ExitApplication());
			notifyIcon.ContextMenuStrip = contextMenu;
		}

		private void RestoreWindow()
		{
			ShowInTaskbar = true;
			WindowState = WindowState.Normal;
			Activate();
		}

		private void ExitApplication()
		{
			isExiting = true;
			Application.Current.Shutdown();
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			if (!isExiting)
			{
				e.Cancel = true;
				WindowState = WindowState.Minimized;
			}
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			if (WindowState == WindowState.Minimized)
			{
				ShowInTaskbar = false;
				notifyIcon.Visible = true;
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			notifyIcon?.Dispose();
			base.OnClosed(e);
		}
	}
}