using System;
using System.ComponentModel.DataAnnotations;

namespace WorkOffice.Domain.Entities.Shared
{
    public partial class AuditTrail : Entity<long>
    {
        public DateTime? ActionDate { get; set; }
        [StringLength(50)]
        public string ActionBy { get; set; }
        [StringLength(2000)]
        public string Details { get; set; }
        [StringLength(100)]
        public string IPAddress { get; set; }
        [StringLength(200)]
        public string HostAddress { get; set; }
        [StringLength(100)]
        public string Page { get; set; }
        [StringLength(100)]
        public string ActionType { get; set; }
    }
}
