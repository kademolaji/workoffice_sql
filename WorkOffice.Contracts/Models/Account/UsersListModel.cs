using WorkOffice.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class UsersListModel
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public RolesEnum UserRole { get; set; }
        public string Biography { get; set; }
        public string Skills { get; set; }
        public bool Status { get; set; }
    }

    public class SearchUserList
    {
        public string SearchQuery { get; set; }
        public RolesEnum? UserRole { get; set; }
    }
}
