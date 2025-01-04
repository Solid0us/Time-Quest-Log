using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models
{
	internal class AuthResponse
	{
        public DateTime Timestamp { get; set; }
		public string Username { get; set; }
		public string Token { get; set; }
		public string Error { get; set; }

        public AuthResponse()
        {
            Timestamp = DateTime.Now;
            Username = string.Empty;
            Token = string.Empty;
            Error = string.Empty;
        }
    }
}
