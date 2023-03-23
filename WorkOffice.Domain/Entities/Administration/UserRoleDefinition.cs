using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class UserRoleDefinition : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long UserRoleDefinitionId { get; set; }
        [Required]
        [StringLength(256)]
        public string RoleName { get; set; }
        public virtual ICollection<UserRoleActivity> UserRoleActivity { get; set; }
    }
}
