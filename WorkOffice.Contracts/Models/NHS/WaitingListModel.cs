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
        [JsonIgnore]
        public string CurrentUsername { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
