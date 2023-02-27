using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
   public class UserAccountSettings : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid UserAccountSettingsId { get; set; }
        public bool EnablePasswordReset { get; set; }

        public int? MinimumRequiredPasswordLength { get; set; }

        public int? MaximumInvalidPasswordAttempts { get; set; }

        public int? AllowPasswordUserAfter { get; set; }

        public int? ExpirePasswordAfter { get; set; }

        public int? MaxPeriodOfUserInactivity { get; set; }

        public int? SessionTimeout { get; set; }
    }
}
