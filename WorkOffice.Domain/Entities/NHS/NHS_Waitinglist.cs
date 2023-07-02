using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class NHS_Waitinglist
    {
        [Key]
        public int WaitinglistId { get; set; }

        public int WaitTypeId { get; set; }

        public int SpecialtyId { get; set; }

        public DateTime? TCIDate { get; set; }

        public DateTime? WaitinglistDate { get; set; }

        [StringLength(50)]
        public string WaitinglistTime { get; set; }

        public int? PatientId { get; set; }

        public int? patientValidationId { get; set; }

        public string Condition { get; set; }

        [StringLength(50)]
        public string WaitinglistStatus { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual NHS_Patient NHS_Patient { get; set; }

        //public virtual NHS_Patient_Validation NHS_Patient_Validation { get; set; }

        //public virtual Specialty Specialty { get; set; }

        //public virtual WaitingType WaitingType { get; set; }
    }
}
