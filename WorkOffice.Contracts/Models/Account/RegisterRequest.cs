using WorkOffice.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int UserRoleId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Range(typeof(bool), "true", "true")]
        public bool AcceptTerms { get; set; }

        public List<UserAdditionalActivityModel> AdditionalActivities { get; set; }
        public long[] UserAccessIds { get; set; }
        public long[] UserRoleIds { get; set; }
        public long ClientId { get; set; }

        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string PhoneNumber { get; set; }
        public int? Accesslevel { get; set; }

    }
}
