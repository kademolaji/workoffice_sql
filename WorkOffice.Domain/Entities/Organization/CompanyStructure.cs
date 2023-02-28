using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public partial class CompanyStructure : Entity
    {
        public CompanyStructure()
        {
            UserAccess = new HashSet<UserAccess>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CompanyStructureId { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(50)]
        public Guid StructureTypeID { get; set; }
        [StringLength(250)]
        public string Country { get; set; }
        [StringLength(250)]
        public string Parent { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        [StringLength(100)]
        public string CompanyHead { get; set; }
        public Guid? ParentID { get; set; }
        [StringLength(250)]
        public string Company { get; set; }
        public virtual ICollection<UserAccess> UserAccess { get; set; }

    }
}
