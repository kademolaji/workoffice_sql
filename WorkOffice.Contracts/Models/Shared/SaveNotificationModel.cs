using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models.Shared
{
    public class SaveNotificationModel
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
