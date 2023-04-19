using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkOffice.Web.Dtos
{
    public class PatientDocumentDto
    {
        public int PatientDocumentId { get; set; }

        public int DocumentTypeId { get; set; }

        public int PatientId { get; set; }

        public string PhysicalLocation { get; set; }

        public string DocumentName { get; set; }

        public DateTime? ClinicDate { get; set; }

        public int? SpecialityId { get; set; }
        public IFormFile File { get; set; }

    }
}
