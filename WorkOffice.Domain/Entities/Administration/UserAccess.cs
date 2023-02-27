using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Domain.Entities
{
   public class UserAccess : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid UserAccessId { get; set; }
        [ForeignKey("CompanyStructure")]
        public long CompanyStructureId { get; set; }
        [ForeignKey("UserAccount")]
        public long UserAccountId { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual CompanyStructure CompanyStructure { get; set; }

    }
}
