using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class UserAccountModel : BaseModel
    {
        public UserAccountModel()
        {
            Activities = new List<UserAdditionalActivityModel>();
        }
        public long UserId { get; set; }
        public string CustomUserCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string ProfilePicture { get; set; }
        public string Country { get; set; }
        public string Biography { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool? LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsFirstLoginAttempt { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public DateTime? NextPasswordChangeDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CurrentLogin { get; set; }
        public bool? CanChangePassword { get; set; }
        public int? Accesslevel { get; set; }
        public long[] UserAccessIds { get; set; }
        public long[] UserRoleIds { get; set; }
        public List<UserAdditionalActivityModel> Activities { get; set; }

    }
}
