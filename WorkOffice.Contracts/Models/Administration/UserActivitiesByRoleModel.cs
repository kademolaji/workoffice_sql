using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
   public class UserActivitiesByRoleModel
    {
        public Guid UserActivityId { get; set; }
        public Guid UserRoleDefinitionId { get; set; }
    }
}
