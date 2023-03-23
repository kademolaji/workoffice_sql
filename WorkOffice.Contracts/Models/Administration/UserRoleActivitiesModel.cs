using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class UserRoleActivitiesModel : BaseModel
    {
        public long UserRoleActivitiesId { get; set; }
        public long UserRoleDefinitionId { get; set; }
        public string UserRoleDefinition { get; set; }
        public long UserActivityParentId { get; set; }
        public string UserActivityParentName { get; set; }
        public long UserActivityId { get; set; }
        public string UserActivityName { get; set; }
        public bool CanEdit { get; set; }
        public bool CanAdd { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
        public bool CanApprove { get; set; }
    }
}
