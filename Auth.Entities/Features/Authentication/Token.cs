using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Entities
{
    [Table("Token", Schema = "dbo")]
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid ProfileId { get; set; }
        public Guid RefreshToken { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsEnabled { get; set; }
    }
}
