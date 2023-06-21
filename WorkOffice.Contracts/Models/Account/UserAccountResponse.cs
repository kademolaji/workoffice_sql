using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
   public class UserAccountResponse
    {
        public long UserId { get; set; }
        public long UserRoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }  
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string LastLogin { get; set; }
    }
}
