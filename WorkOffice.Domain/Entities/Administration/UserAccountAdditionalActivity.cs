using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
   public class UserAccountAdditionalActivity : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long UserAccountAdditionalActivityId { get; set; }
        [ForeignKey("UserAccount")]
        public long UserAccountId { get; set; }
        [ForeignKey("UserActivity")]
        public long UserActivityId { get; set; }

        public bool? CanEdit { get; set; }

        public bool? CanAdd { get; set; }

        public bool? CanView { get; set; }

        public bool? CanDelete { get; set; }

        public bool? CanApprove { get; set; }
        public virtual UserActivity UserActivity { get; set; }

        public virtual UserAccount UserAccount { get; set; }

    }
}
