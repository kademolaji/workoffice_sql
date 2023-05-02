using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkOffice.Web.Dtos
{
    public class DiagnosticResultDto
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
        public IFormFile File { get; set; }

    }
}
