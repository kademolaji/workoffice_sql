using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkOffice.Web.Dtos
{
    public class ReferralDto
    {
        public int ReferralId { get; set; }
        public int PatientId { get; set; }
        public int SpecialtyId { get; set; }
        public int ConsultantId { get; set; }
        public string ConsultantName { get; set; }
        public string DocumentExtension { get; set; }
        public string DocumentName { get; set; }
        public byte[] DocumentFile { get; set; }
        public DateTime ReferralDate { get; set; }
        public bool? Active { get; set; }
        public string FullName { get; set; }
        public string CurrentUserName { get; set; }
        public IFormFile File { get; set; }

    }
}
