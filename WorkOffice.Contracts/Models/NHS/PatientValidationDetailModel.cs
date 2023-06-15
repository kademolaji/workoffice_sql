using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class PatientValidationDetailModel
    {
        public int PatientValidationDetailsId { get; set; }

        public int PatientValidationId { get; set; }

        public int? PathWayStatusId { get; set; }

        public int? SpecialtyId { get; set; }

        public DateTime Date { get; set; }

        public int? ConsultantId { get; set; }

        public DateTime? EndDate { get; set; }

        public int? PatientId { get; set; }

        public string Activity { get; set; }

        public bool? Active { get; set; }

        public string Type { get; set; }
    }
}
