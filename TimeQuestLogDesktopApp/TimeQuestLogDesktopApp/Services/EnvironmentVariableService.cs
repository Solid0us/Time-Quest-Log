using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;

namespace TimeQuestLogDesktopApp.Services
{
	internal class EnvironmentVariableService
	{
		public string ApiBaseUrl { get; private set; }

		public EnvironmentVariableService()
		{
			var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env");

			LoadEnvFile(envPath);

			// Read and store relevant variables
			ApiBaseUrl = GetRequiredVariable("API_BASE_URL");
		}

		private void LoadEnvFile(string filePath)
		{
			try
			{
				Env.Load(filePath);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Failed to load environment file at {filePath}.", ex);
			}
		}

		private string GetRequiredVariable(string key)
		{
			var value = Environment.GetEnvironmentVariable(key);

			if (string.IsNullOrEmpty(value))
			{
				throw new InvalidOperationException($"Environment variable '{key}' is missing or empty.");
			}

			return value;
		}

		public string GetOptionalVariable(string key, string defaultValue = "")
		{
			return Environment.GetEnvironmentVariable(key) ?? defaultValue;
		}
	}
}
