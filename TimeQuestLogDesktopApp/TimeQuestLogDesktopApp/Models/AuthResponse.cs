using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models
{
	internal class AuthResponse
	{
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
		public string Username { get; set; }
		public string Token { get; set; }
        public string RefreshToken {  get; set; }
		public string Error { get; set; }

        public AuthResponse()
        {
            UserId = string.Empty;
            Timestamp = DateTime.Now;
            Username = string.Empty;
            Token = string.Empty;
            Error = string.Empty;
            RefreshToken = string.Empty;
        }
    }
}
