using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Domain.Entities.Shared
{
    public class Notification : Entity<long>
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
    }
}
