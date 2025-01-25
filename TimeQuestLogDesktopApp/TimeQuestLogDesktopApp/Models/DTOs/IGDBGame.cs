using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models.DTOs
{
    class IGDBGame
    {
        public int id { get; set; }
        public IGDBCover? cover { get; set; }
        public int first_release_date { get; set; }
        public List<IGDBGenre>? genres { get; set; }
        public string name { get; set; }
    }
}
