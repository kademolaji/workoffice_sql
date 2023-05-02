using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class NHS_Diagnostic
    {
        public NHS_Diagnostic()
        {
            NHS_Patient = new HashSet<NHS_Patient>();
            Specialty = new HashSet<Specialty>();
        }
        [Key]
        public int DiagnosticId { get; set; }

        public int PatientId { get; set; }

        public int SpecialtyId { get; set; }

        [Required]
        [StringLength(550)]
        public string Problem { get; set; }

        public DateTime DTD { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string ConsultantName { get; set; }


        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<NHS_Patient> NHS_Patient { get; set; }

        public virtual ICollection<Specialty> Specialty { get; set; }
    }
}
