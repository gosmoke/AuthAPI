using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Auth.Entities
{
    [Table("Profile", Schema = "dbo")]
    public class Profile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Application")]
        public int ApplicationId { get; set; }
        [ForeignKey("UserDetail")]
        public int UserDetailId { get; set; }
        public Guid ProfileId { get; set; }
        public string AccountId { get; set; }
        public DateTime LastLoginOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Application Application { get; set; }
        public virtual UserDetail UserDetail { get; set; }
    }
}
