using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class NHS_Patient
    {
        public NHS_Patient()
        {
            NHS_Appointment = new HashSet<NHS_Appointment>();
            NHS_Waitinglist = new HashSet<NHS_Waitinglist>();
        }
        [Key]
        public int PatientId { get; set; }

        [Required]
        [StringLength(50)]
        public string DistrictNumber { get; set; }

        [Required]
        [StringLength(150)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(150)]
        public string LastName { get; set; }

        [StringLength(150)]
        public string MiddleName { get; set; }

        public DateTime DOB { get; set; }

        public int Age { get; set; }

        [StringLength(550)]
        public string Address { get; set; }

        [StringLength(50)]
        public string PhoneNo { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Sex { get; set; }

        [StringLength(50)]
        public string PostalCode { get; set; }

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

        public virtual ICollection<NHS_Appointment> NHS_Appointment { get; set; }
        public virtual ICollection<NHS_Waitinglist> NHS_Waitinglist { get; set; }
    }
}
