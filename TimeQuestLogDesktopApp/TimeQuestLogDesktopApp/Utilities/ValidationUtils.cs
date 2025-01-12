using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Utilities
{
    class ValidationUtils
    {
        public static bool IsValidEmail(string email)
        {
			var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

			var regex = new Regex(pattern);
			return regex.IsMatch(email);
		}
    }
}
