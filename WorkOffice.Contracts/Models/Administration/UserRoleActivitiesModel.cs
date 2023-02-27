using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class UserRoleActivitiesModel : BaseModel
    {
        public Guid UserRoleActivitiesId { get; set; }
        public Guid UserRoleDefinitionId { get; set; }
        public string UserRoleDefinition { get; set; }
        public Guid UserActivityParentId { get; set; }
        public string UserActivityParentName { get; set; }
        public Guid UserActivityId { get; set; }
        public string UserActivityName { get; set; }
        public bool CanEdit { get; set; }
        public bool CanAdd { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
        public bool CanApprove { get; set; }
    }
}
