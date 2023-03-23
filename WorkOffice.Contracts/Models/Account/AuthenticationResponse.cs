using WorkOffice.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WorkOffice.Contracts.Models
{
   public class AuthenticationResponse
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string Token { get; set; }
        public string UserRole { get; set; }
        public bool IsVerified { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public string FullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
