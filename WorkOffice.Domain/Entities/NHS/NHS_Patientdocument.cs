﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class NHS_Patientdocument
    {
        [Key]
        public int PatientDocumentId { get; set; }

        public int DocumentTypeId { get; set; }

        public int PatientId { get; set; }

        [StringLength(500)]
        public string PhysicalLocation { get; set; }

        [Required]
        [StringLength(255)]
        public string DocumentName { get; set; }

        [Required]
        [StringLength(50)]
        public string DocumentExtension { get; set; }

        [Required]
        public byte[] DocumentFile { get; set; }

        public DateTime? ClinicDate { get; set; }

        public DateTime? DateUploaded { get; set; }

        public int? SpecialtyId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
