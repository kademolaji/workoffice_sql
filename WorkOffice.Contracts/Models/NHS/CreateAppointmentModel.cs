using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WorkOffice.Contracts.Models
{
   public class CreateAppointmentModel
    {
        public int AppointmentId { get; set; }

        public int AppTypeId { get; set; }

        public int? StatusId { get; set; }

        public int? SpecialityId { get; set; }

        public DateTime BookDate { get; set; }

        public DateTime AppDate { get; set; }
        public string AppTime { get; set; }

        public int? ConsultantId { get; set; }

        public int? HospitalId { get; set; }

        public int? WardId { get; set; }

        public int? DepartmentId { get; set; }

        public int PatientId { get; set; }

        public int? PatientValidationId { get; set; }

        public string Comments { get; set; }

        public string AppointmentStatus { get; set; }

        public string CancellationReason { get; set; }

        [JsonIgnore]
        public string CurrentUserName { get; set; }
    
    }
}
