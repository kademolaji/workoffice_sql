using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
   public class UserActionModel
    {
        public Guid ActivityId { get; set; }
        public bool CanEdit { get; set; }
        public bool CanAdd { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
        public bool CanApprove { get; set; }
    }
}
