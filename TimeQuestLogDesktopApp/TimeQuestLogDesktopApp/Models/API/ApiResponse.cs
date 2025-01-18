using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeQuestLogDesktopApp.Models.API
{
    internal class ApiResponse<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public DateTime Timestamp { get; set; }
        public List<ApiError>? Errors { get; set; }

        public ApiResponse(T data)
        {
            Errors = new List<ApiError>();
            Status = string.Empty;
            Message = string.Empty;
            Data = data;
            Timestamp = DateTime.Now;
        }
    }
}
