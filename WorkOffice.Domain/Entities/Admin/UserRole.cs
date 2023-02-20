using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WorkOffice.Domain.Entities.Shared;

namespace WorkOffice.Domain.Entities.Admin
{
    public class UserRole: Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid USerRoleId { get; set; }
        public string Name { get; set; }

    }
}
