using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
   public class UserActivity : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid UserActivityId { get; set; }
        [Required]
        [StringLength(256)]
        public string UserActivityName { get; set; }
        [ForeignKey("UserActivityParent")]
        public Guid UserActivityParentId { get; set; }
        public virtual UserActivityParent UserActivityParent { get; set; }
    }
}
