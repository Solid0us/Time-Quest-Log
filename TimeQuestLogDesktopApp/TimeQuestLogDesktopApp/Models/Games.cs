using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models
{
	internal class Games
	{
		public int Id { get; set; }
        public string CoverUrl { get; set; }
        public string Name { get; set; }

        public Games(int id, string coverUrl, string name)
        {
            Id = id;
            CoverUrl = coverUrl;
            Name = name;
        }

        public Games() { }
    }
}
