using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class SearchAuditTrailModel
    {
        public DateTime? ActionDate { get; set; }
        public string ActionBy { get; set; }
        public string Details { get; set; }
        public string IPAddress { get; set; }
        public string HostAddress { get; set; }
        public string Page { get; set; }
        public string ActionType { get; set; }
    }
}
