using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class NHS_Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public int AppTypeId { get; set; }

        public int? StatusId { get; set; }

        public int? SpecialtyId { get; set; }

        public DateTime BookDate { get; set; }

        public DateTime AppDate { get; set; }

        [StringLength(50)]
        public string AppTime { get; set; }

        public int? ConsultantId { get; set; }

        public int? HospitalId { get; set; }

        public int? WardId { get; set; }

        public int? DepartmentId { get; set; }

        public int PatientId { get; set; }

        public int? PatientValidationId { get; set; }

        public string Comments { get; set; }

        [StringLength(50)]
        public string AppointmentStatus { get; set; }

        [StringLength(50)]
        public string CancellationReason { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual AppType AppType { get; set; }

        //public virtual Consultant Consultant { get; set; }

        //public virtual Hospital Hospital { get; set; }

        //public virtual NHS_Patient NHS_Patient { get; set; }

        //public virtual NHS_Patient_Validation NHS_Patient_Validation { get; set; }

        //public virtual Specialty Specialty { get; set; }

        //public virtual Ward Ward { get; set; }
    }
}
