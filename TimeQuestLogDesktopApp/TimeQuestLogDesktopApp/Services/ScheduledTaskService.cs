using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TimeQuestLogDesktopApp.Services
{
	internal class ScheduledTaskService
	{
		private readonly string _appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{AppDomain.CurrentDomain.FriendlyName}.exe");
		private readonly string _appDirectory = AppDomain.CurrentDomain.BaseDirectory;
		private const string TaskName = "TimeQuestLogStartup";
		private const string TaskDescription = "Starts TimeQuest Log application when user logs in";
		private static ScheduledTaskService _instance;
		private static readonly object lockObject = new object();

		private ScheduledTaskService() { }

		public static ScheduledTaskService Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (lockObject)
					{
						if (_instance == null)
						{
							_instance = new ScheduledTaskService();
						}
					}
				}
				return _instance;
			}
		}

		public bool IsStartupEnabled()
		{
			try
			{
				ProcessStartInfo psi = new ProcessStartInfo("schtasks", $"/query /tn \"{TaskName}\" /fo LIST")
				{
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};

				using (Process process = new Process { StartInfo = psi })
				{
					process.Start();
					string output = process.StandardOutput.ReadToEnd();
					process.WaitForExit();

					// Check if the task exists and is enabled
					return process.ExitCode == 0 &&
						   !output.Contains("Disabled") &&
						   output.Contains(TaskName);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error checking startup status: {ex.Message}");
				return false;
			}
		}

		public bool EnableStartup()
		{
			try
			{
				if (!IsRunningInAdminMode())
				{
					RestartWithAdmin();
					return false;
				}

				try { DisableStartup(false); } catch { /* Ignore if task doesn't exist */ }

				string xmlPath = Path.Combine(Path.GetTempPath(), $"{TaskName}.xml");
				string xml = CreateTaskXml();
				File.WriteAllText(xmlPath, xml);

				string command = $"/create /tn \"{TaskName}\" /xml \"{xmlPath}\" /f";
				bool result = ExecuteCommand(command);

				try { File.Delete(xmlPath); } catch { /* Ignore cleanup errors */ }

				return result;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error enabling startup: {ex.Message}");
				return false;
			}
		}

		public bool DisableStartup(bool requireAdmin = true)
		{
			try
			{
				if (requireAdmin && !IsRunningInAdminMode())
				{
					RestartWithAdmin();
					return false;
				}

				string command = $"/delete /tn \"{TaskName}\" /f";
				return ExecuteCommand(command);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error disabling startup: {ex.Message}");
				return false;
			}
		}

		private bool ExecuteCommand(string arguments)
		{
			try
			{
				ProcessStartInfo psi = new ProcessStartInfo("schtasks", arguments)
				{
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};

				using (Process process = new Process { StartInfo = psi })
				{
					process.Start();
					string output = process.StandardOutput.ReadToEnd();
					string error = process.StandardError.ReadToEnd();
					process.WaitForExit();

					if (process.ExitCode != 0)
					{
						Debug.WriteLine($"Command failed: {error}");
						return false;
					}

					return true;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Command execution error: {ex.Message}");
				return false;
			}
		}
		/// <summary>
		/// Creates Xml task configuration for support across Windows operation systems.
		/// </summary>
		/// <returns></returns>
		private string CreateTaskXml()
		{
			string escapedPath = _appPath.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
			string escapedDir = _appDirectory.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");

			return $@"<?xml version=""1.0"" encoding=""UTF-16""?>
					<Task version=""1.4"" xmlns=""http://schemas.microsoft.com/windows/2004/02/mit/task"">
					  <RegistrationInfo>
						<Date>{DateTime.Now:yyyy-MM-ddTHH:mm:ss}</Date>
						<Author>{Environment.UserName}</Author>
						<Description>{TaskDescription}</Description>
						<URI>\{TaskName}</URI>
					  </RegistrationInfo>
					  <Triggers>
						<LogonTrigger>
						  <Enabled>true</Enabled>
						  <UserId>{Environment.UserDomainName}\{Environment.UserName}</UserId>
						</LogonTrigger>
					  </Triggers>
					  <Principals>
						<Principal id=""Author"">
						  <UserId>{Environment.UserDomainName}\{Environment.UserName}</UserId>
						  <LogonType>InteractiveToken</LogonType>
						  <RunLevel>HighestAvailable</RunLevel>
						</Principal>
					  </Principals>
					  <Settings>
						<MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>
						<DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
						<StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
						<AllowHardTerminate>true</AllowHardTerminate>
						<StartWhenAvailable>true</StartWhenAvailable>
						<RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>
						<IdleSettings>
						  <StopOnIdleEnd>false</StopOnIdleEnd>
						  <RestartOnIdle>false</RestartOnIdle>
						</IdleSettings>
						<AllowStartOnDemand>true</AllowStartOnDemand>
						<Enabled>true</Enabled>
						<Hidden>false</Hidden>
						<RunOnlyIfIdle>false</RunOnlyIfIdle>
						<DisallowStartOnRemoteAppSession>false</DisallowStartOnRemoteAppSession>
						<UseUnifiedSchedulingEngine>true</UseUnifiedSchedulingEngine>
						<WakeToRun>false</WakeToRun>
						<ExecutionTimeLimit>PT0S</ExecutionTimeLimit>
						<Priority>7</Priority>
					  </Settings>
					  <Actions Context=""Author"">
						<Exec>
						  <Command>{escapedPath}</Command>
						  <WorkingDirectory>{escapedDir}</WorkingDirectory>
						</Exec>
					  </Actions>
					</Task>";
		}

		private bool IsRunningInAdminMode()
		{
			using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
			{
				WindowsPrincipal principal = new WindowsPrincipal(identity);
				return principal.IsInRole(WindowsBuiltInRole.Administrator);
			}
		}

		private void RestartWithAdmin()
		{
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = _appPath,
				Arguments = "/elevate",
				UseShellExecute = true,
				Verb = "runas"
			};

			try
			{
				Process.Start(psi);
				Environment.Exit(0);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Unable to restart with administrator privileges. Please run the application as administrator.\n\nError: {ex.Message}",
					"Elevation Required",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
			}
		}
	}
}