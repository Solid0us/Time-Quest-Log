using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models
{
	internal class Genres
	{
		public int Id { get; set; }
        public string Name { get; set; }

        public Genres(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Genres()
        {
            
        }
    }
}
