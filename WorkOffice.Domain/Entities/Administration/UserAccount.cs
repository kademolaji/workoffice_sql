using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class UserAccount : Entity
    {
        public UserAccount()
        {
            UserAccess = new HashSet<UserAccess>();
            UserAccountRole = new HashSet<UserAccountRole>();
            UserAccountAdditionalActivity = new HashSet<UserAccountAdditionalActivity>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long UserId { get; set; }
        public string CustomUserCode { get; set; }
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
        public bool? EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool IsFirstLoginAttempt { get; set; }
        [StringLength(256)]
        public string SecurityQuestion { get; set; }
        [StringLength(256)]
        public string SecurityAnswer { get; set; }
        public DateTime? NextPasswordChangeDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastActive { get; set; }
        public DateTime? CurrentLogin { get; set; }
        public bool? CanChangePassword { get; set; }
        public int? Accesslevel { get; set; }

        public virtual ICollection<UserAccess> UserAccess { get; set; }

        public virtual ICollection<UserAccountRole> UserAccountRole { get; set; }

        public virtual ICollection<UserAccountAdditionalActivity> UserAccountAdditionalActivity { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
