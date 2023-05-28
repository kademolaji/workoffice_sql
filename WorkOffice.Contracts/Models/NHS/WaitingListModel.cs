using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WorkOffice.Contracts.Models
{
    public class WaitingListModel
    {
        public int WaitinglistId { get; set; }

        public int WaitTypeId { get; set; }

        public int SpecialityId { get; set; }

        public DateTime? TCIDate { get; set; }

        public DateTime WaitinglistDate { get; set; }

        public string WaitinglistTime { get; set; }

        public int? PatientId { get; set; }

        public int? patientValidationId { get; set; }

        public string Condition { get; set; }

        public string WaitinglistStatus { get; set; }
        public string DistrictNumber { get; set; }
        public string PathWayNumber { get; set; }
      
        [JsonIgnore]
        public string CurrentUsername { get; set; }
    }
}
