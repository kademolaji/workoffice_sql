using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace WorkOffice.Contracts.Models
{
   public class DiagnosticResultModel
    {
        public int DiagnosticResultId { get; set; }
        public int DiagnosticId { get; set; }
        public int PatientId { get; set; }
        public string Problem { get; set; }
        public string ConsultantName { get; set; }
        public string DocumentName { get; set; }       
        public string DocumentExtension { get; set; }
        public byte[] DocumentFile { get; set; }
        public DateTime? TestResultDate { get; set; }
        public DateTime? DateUploaded { get; set; }
        public int? SpecialityId { get; set; }
        public string Speciality { get; set; }
        public bool? Active { get; set; }
        public string FullName { get; set; }
        [JsonIgnore]
        public string CurrentUserName { get; set; }
    
    }
}
