using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
   public class UserAdditionalActivityModel : BaseModel
    {
        public long UserAdditionalActivityId { get; set; }
        public long UserActivityId { get; set; }

        public bool? CanEdit { get; set; }

        public bool? CanAdd { get; set; }

        public bool? CanView { get; set; }

        public bool? CanDelete { get; set; }

        public bool? CanApprove { get; set; }
    }
}
