using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
   public class UserRoleDefinitionModel : BaseModel
    {
        public long UserRoleDefinitionId { get; set; }
        public string RoleName { get; set; }
        public int UserCount { get; set; }

    }
}
