using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class SaveNotificationModel
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
