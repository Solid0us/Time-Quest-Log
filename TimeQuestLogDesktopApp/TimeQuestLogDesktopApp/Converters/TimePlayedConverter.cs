using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TimeQuestLogDesktopApp.Models.DTOs;

namespace TimeQuestLogDesktopApp.Converters
{
	internal class TimePlayedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// Ensure the value is a GameSessionsDTO object
			if (value is GameSessionsDTO session)
			{
				// Calculate the time difference if EndTime is not null
				if (session.EndTime.HasValue)
				{
					TimeSpan timePlayed = session.EndTime.Value - session.StartTime;
					return $"{(int)timePlayed.TotalHours} hr {timePlayed.Minutes} min {timePlayed.Seconds} sec";
				}
				else
				{
					return "In Progress"; // If EndTime is null, the session is still ongoing
				}
			}

			return string.Empty; // Fallback for invalid data
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
