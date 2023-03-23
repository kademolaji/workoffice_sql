using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
  public  class UserRoleAndActivityModel : BaseModel
    {
        public long UserRoleAndActivityId { get; set; }
        public string UserRoleDefinition { get; set; }
        public List<UserRoleActivitiesModel> activities { get; set; }
    }
}
