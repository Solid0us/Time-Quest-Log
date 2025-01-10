using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models
{
	internal class Users
	{
        public string Id { get; set; }
		public string Username { get; set; }

        public Users(string id, string username)
        {
            Id = id;
            Username = username;
        }
    }
}
