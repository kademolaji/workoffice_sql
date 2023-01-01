using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Domain.Entities.Shared;

namespace WorkOffice.Domain.Entities.Account
{
    public class User : Entity<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime CreationDate { get; set; }
        public int RoleId { get; set; }
        public string ProfilePicture { get; set; }
        public string Country { get; set; }
        public string Biography { get; set; }
        public bool Disabled { get; set; }
        public DateTime DeletionDate { get; set; }
        public bool AcceptTerms { get; set; }
        public string VerificationToken { get; set; }
        public DateTime? Verified { get; set; }
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
