using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models.DTOs
{
    class RefreshTokenResponse
    {
        public string? token { get; set; }

        public RefreshTokenResponse()
        {
            token = null;
        }
    }
}
