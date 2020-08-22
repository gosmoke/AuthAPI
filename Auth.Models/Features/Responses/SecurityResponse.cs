using Microsoft.AspNetCore.Http;
using System;

namespace Auth.Models
{
    public class SecurityResponse : IServiceResponse<string>
    {
        public string Content { get; set; }
        public int ResponseCode { get; set; } = StatusCodes.Status200OK;
        public string RefreshToken { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
