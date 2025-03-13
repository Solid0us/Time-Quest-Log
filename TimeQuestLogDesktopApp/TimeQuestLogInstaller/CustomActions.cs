using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TimeQuestLogInstaller
{
	[RunInstaller(true)]
	public partial class CustomActions : Installer
	{
		public CustomActions()
		{
			InitializeComponent();
		}

		public override void Install(IDictionary stateSaver)
		{
			try
			{
				// Get the target directory from the installer
				string targetDir = "C:\\Program Files\\SolidCompany";

				// If this is an upgrade, the directory should already exist
				if (Directory.Exists(targetDir))
				{
					string dbPath = Path.Combine(targetDir, "timequestlog.db");

					if (File.Exists(dbPath))
					{
						// Create backup with timestamp
						string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
						string backupPath = Path.Combine("C:\\Program Files\\SolidCompany", $"timequestlog.db.{timestamp}.bak");

						// Copy the database file
						File.Copy(dbPath, backupPath, true);
					}
				}
			}
			catch (Exception ex)
			{
				// Log but continue
			}
			base.Install(stateSaver);
			
		}

		public override void Commit(IDictionary savedState)
		{
			base.Commit(savedState);
			// Your commit logic (e.g., archiving)
		}

		public override void Rollback(IDictionary savedState)
		{
			base.Rollback(savedState);
			// Your rollback logic
		}

		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall(savedState);
			// Your uninstall logic
		}
	}
}
