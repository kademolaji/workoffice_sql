using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
   public class UserAccessModel : BaseModel
    {
        public Guid UserAccessId { get; set; }
        public Guid CompanyStructureId { get; set; }
    }
}
