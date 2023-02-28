using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
   public class UserActivityModel
    {
        public Guid UserActivityId { get; set; }
        public string UserActivityName { get; set; }
        public Guid UserActivityParentId { get; set; }
    }
}

