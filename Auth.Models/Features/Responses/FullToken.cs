using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Models
{
    public class FullToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
