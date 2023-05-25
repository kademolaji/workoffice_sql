using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class NHS_Patient_Validation_Detail
    {
        [Key]
        public int PatientValidationDetailsId { get; set; }

        public int PatientValidationId { get; set; }

        public int? PathWayStatusId { get; set; }

        public int? SpecialtyId { get; set; }

        public DateTime Date { get; set; }

        public int? ConsultantId { get; set; }

        public DateTime? EndDate { get; set; }

        public int? PatientId { get; set; }

        [StringLength(50)]
        public string Activity { get; set; }

        public bool? Active { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual NHS_Patient NHS_Patient { get; set; }

        //public virtual NHS_Patient_Validation NHS_Patient_Validation { get; set; }
    }
}
