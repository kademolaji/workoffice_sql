using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public partial class StructureDefinition : Entity
    {
        public StructureDefinition()
        {
            CompanyStructure = new HashSet<CompanyStructure>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid StructureDefinitionId { get; set; }
        [StringLength(50)]
        public string Definition { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        public int Level { get; set; }
        public virtual ICollection<CompanyStructure> CompanyStructure { get; set; }
    }
}
