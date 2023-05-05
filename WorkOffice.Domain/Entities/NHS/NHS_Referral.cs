using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class NHS_Referral
    {
        public NHS_Referral()
        {
            NHS_Patient = new HashSet<NHS_Patient>();
            Specialty = new HashSet<Specialty>();
            Consultant = new HashSet<Consultant>();
        }
        [Key]
        public int ReferralId { get; set; }

        public int PatientId { get; set; }

        public int SpecialtyId { get; set; }

        public int ConsultantId { get; set; }
        public DateTime ReferralDate { get; set; }

        [StringLength(50)]
        public string ConsultantName { get; set; }

        [StringLength(50)]
        public string DocumentExtension { get; set; }

        [StringLength(50)]
        public string DocumentName { get; set; }
        [Required]
        public byte[] DocumentFile { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<NHS_Patient> NHS_Patient { get; set; }
        public virtual ICollection<Consultant> Consultant { get; set; }
        public virtual ICollection<Specialty> Specialty { get; set; }
    }
}
