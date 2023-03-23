using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
   public class UserActivityModel
    {
        public long UserActivityId { get; set; }
        public string UserActivityName { get; set; }
        public long UserActivityParentId { get; set; }
    }
}

