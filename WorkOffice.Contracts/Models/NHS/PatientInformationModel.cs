using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WorkOffice.Contracts.Models
{
   public class PatientInformationModel
    {
        public int PatientId { get; set; }
        public string DistrictNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string PostalCode { get; set; }
        public string NHSNumber { get; set; }
        public bool? Active { get; set; }
        public string FullName { get; set; }
        [JsonIgnore]
        public string CurrentUserName { get; set; }
    
    }
}
