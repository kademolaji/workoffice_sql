using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class PatientDocumentModel
    {
        public int PatientDocumentId { get; set; }

        public int DocumentTypeId { get; set; }

        public int PatientId { get; set; }

        public string PhysicalLocation { get; set; }

        public string DocumentName { get; set; }

        public string DocumentExtension { get; set; }

        public byte[] DocumentFile { get; set; }

        public DateTime? ClinicDate { get; set; }

        public DateTime? DateUploaded { get; set; }

        public int? SpecialityId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
