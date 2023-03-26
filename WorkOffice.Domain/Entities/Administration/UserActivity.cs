using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
   public class UserActivity : Entity
    {
        [Key]
        public long UserActivityId { get; set; }
        [Required]
        [StringLength(256)]
        public string UserActivityName { get; set; }
        [ForeignKey("UserActivityParent")]
        public long UserActivityParentId { get; set; }
        public virtual UserActivityParent UserActivityParent { get; set; }
    }
}
