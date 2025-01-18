using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models.API
{
	internal class ApiError
	{
        public string Field { get; set; }
		public string Message { get; set; }

        public ApiError()
        {
            Field = string.Empty;
            Message = string.Empty;
        }
    }
}
