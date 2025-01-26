using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models.DTOs
{
    class RefreshTokenRequest
    {
        public string? refreshToken { get; set; }
        public string username { get; set; }

        public RefreshTokenRequest()
        {
            
        }
    }
}
