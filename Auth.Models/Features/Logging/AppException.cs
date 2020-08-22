using System;

namespace Auth.Models
{
    public class AppException
    {
        public int Id { get; set; }
        public Guid CreatedBy { get; set; }
        public int Severity { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
