using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class NHS_Patient_Validation
    {
        //public NHS_Patient_Validation()
        //{
        //    NHS_Appointment = new HashSet<NHS_Appointment>();
        //    NHS_Patient_Validation_Detail = new HashSet<NHS_Patient_Validation_Detail>();
        //    NHS_Waitinglist = new HashSet<NHS_Waitinglist>();
        //    NHS_Patient = new HashSet<NHS_Patient>();
        //    Specialty = new HashSet<Specialty>();
        //}

        [Key]
        public int PatientValidationId { get; set; }
        public int PatientId { get; set; }

        [Required]
        [StringLength(150)]
        public string PathWayNumber { get; set; }

        [StringLength(150)]
        public string PathWayCondition { get; set; }

        public int? PathWayStatusId { get; set; }

        public string PathWayStatusIdCode { get; set; }

        public DateTime PathWayStartDate { get; set; }

        public DateTime? PathWayEndDate { get; set; }

        public int SpecialtyId { get; set; }

        [StringLength(50)]
        public int RTTId { get; set; }

        [StringLength(550)]
        public string RTTWait { get; set; }

        [StringLength(50)]
        public string DistrictNumber { get; set; }

        [StringLength(50)]
        public string NHSNumber { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual ICollection<NHS_Appointment> NHS_Appointment { get; set; }

        //public virtual PathwayStatus PathwayStatus { get; set; }

        //public virtual ICollection<NHS_Patient_Validation_Detail> NHS_Patient_Validation_Detail { get; set; }

        //public virtual ICollection<NHS_Patient> NHS_Patient { get; set; }

        //public virtual ICollection<Specialty> Specialty { get; set; }

        //public virtual ICollection<NHS_Waitinglist> NHS_Waitinglist { get; set; }
    }
}
