using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Models
{
    public class LoginRequest
    {
        public string AccountId { get; set; }
        public string AppToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
