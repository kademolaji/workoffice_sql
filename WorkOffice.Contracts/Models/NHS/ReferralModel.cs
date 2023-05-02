using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace WorkOffice.Contracts.Models
{
   public class ReferralModel
    {
        public int ReferralId { get; set; }
        public int PatientId { get; set; }
        public int SpecialtyId { get; set; }
        public int ConsultantId { get; set; }
        public string ConsultantName { get; set; }
        public string DocumentName { get; set; }
        public string DocumentExtension { get; set; }
        public byte[] DocumentFile { get; set; }
        public DateTime ReferralDate { get; set; }
        public bool? Active { get; set; }
        public string FullName { get; set; }
        [JsonIgnore]
        public string CurrentUserName { get; set; }


    }
}
