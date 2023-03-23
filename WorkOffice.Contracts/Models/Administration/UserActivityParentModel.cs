using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.Models
{
   public class UserActivityParentModel
    {
        public long UserActivityParentId { get; set; }
        public string UserActivityParentName { get; set; }
        public List<UserActivityModel> activities { get; set; }
    }
}
