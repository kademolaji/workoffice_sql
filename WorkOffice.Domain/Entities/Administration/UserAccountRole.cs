using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
   public class UserAccountRole : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long UserAccountRoleId { get; set; }
        [ForeignKey("UserAccount")]
        public long UserAccountId { get; set; }
        [ForeignKey("UserRoleDefinition")]
        public long UserRoleDefinitionId { get; set; }
        public virtual UserRoleDefinition UserRoleDefinition { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
