using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
   public class UserAccessModel : BaseModel
    {
        public long  UserAccessId { get; set; }
        public long CompanyStructureId { get; set; }
    }
}
