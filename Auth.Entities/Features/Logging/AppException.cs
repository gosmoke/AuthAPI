using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Entities
{
    [Table("AppException", Schema = "dbo")]
    public class AppException
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid CreatedBy { get; set; }
        public int Severity { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

    }
}
