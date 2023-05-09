using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace WorkOffice.Contracts.Models
{
   public class DiagnosticModel
    {
        public int DiagnosticId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int SpecialtyId { get; set; }
        public string Specialty { get; set; }
        public string Problem { get; set; }
        public DateTime DTD { get; set; }
        public string Status { get; set; }
        public string ConsultantName { get; set; }
        public bool? Active { get; set; }
        [JsonIgnore]
        public string CurrentUserName { get; set; }
    
    }
}
