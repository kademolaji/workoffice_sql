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
        public Guid UserAccountRoleId { get; set; }
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }
        [ForeignKey("UserRoleDefinition")]
        public Guid UserRoleDefinitionId { get; set; }
        public virtual UserRoleDefinition UserRoleDefinition { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
