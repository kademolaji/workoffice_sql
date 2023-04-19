using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

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

        public string Speciality { get; set; }
        public string ConsultantName { get; set; }
        [JsonIgnore]
        public string CurrentUserName { get; set; }
    }
}
