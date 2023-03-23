using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
   public class UserActivitiesByRoleModel
    {
        public long UserActivityId { get; set; }
        public long UserRoleDefinitionId { get; set; }
    }
}
