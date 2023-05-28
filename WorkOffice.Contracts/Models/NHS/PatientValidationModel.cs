using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace WorkOffice.Contracts.Models
{
   public class PatientValidationModel
    {

        public PatientValidationModel()
        {
            patientdetails = new List<PatientValidationDetailsModel>();
        }
        public int PatientValidationId { get; set; }
        public string PathWayNumber { get; set; }
        public string PathWayCondition { get; set; }
        public int? PathWayStatusId { get; set; }
        public string PathWayStatusIdCode { get; set; }
        public DateTime PathWayStartDate { get; set; }
        public DateTime? PathWayEndDate { get; set; }
        public int PatientId { get; set; }
        public int SpecialtyId { get; set; }
        public int RTTId { get; set; }
        public string RTTWait { get; set; }
        public string DistrictNumber { get; set; }
        public string NHSNumber { get; set; }
        public bool? Active { get; set; }
        public string FullName { get; set; }
        [JsonIgnore]
        public string CurrentUserName { get; set; }
        public string RTTCode { get; set; }
        public string RTTName { get; set; }
        public string SpecialityCode { get; set; }
        public string SpecialityName { get; set; }
        public string PathWayStatusCode { get; set; }
        public string PathWayStatusName { get; set; }
        public string PatientName { get; set; }
        public List<PatientValidationDetailsModel> patientdetails { get; set; }

    }

    public class PatientValidationDetailsModel
    {
        public int PatientValidationDetailsId { get; set; }
        public int PatientValidationId { get; set; }
        public int? PathWayStatusId { get; set; }
        public int? SpecialityId { get; set; }
        public DateTime Date { get; set; }
        public int? ConsultantId { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PatientId { get; set; }
        public string Activity { get; set; }
        public bool? Active { get; set; }
        public string FullName { get; set; }
        [JsonIgnore]
        public string CurrentUserName { get; set; }
        public string RTTCode { get; set; }
        public string RTTName { get; set; }
        public string SpecialityCode { get; set; }
        public string SpecialityName { get; set; }
        public string PathWayStatusCode { get; set; }
        public string PathWayStatusName { get; set; }
        public string ConsultantCode { get; set; }
        public string ConsultantName { get; set; }
        public string WardCode { get; set; }
        public string WardName { get; set; }
        public string HospitalCode { get; set; }
        public string HospitalName { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityName { get; set; }
    }
}
