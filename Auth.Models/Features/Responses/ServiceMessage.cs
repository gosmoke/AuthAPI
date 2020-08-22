using Auth.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Models
{
    public class ServiceMessage
    {
        public Severity Severity { get; set; }
        public string Message { get; set; }
    }
}
