using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class CustomIdentityFormatSetting : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CustomIdentityFormatSettingId { get; set; }
        public int Prefix { get; set; }
        public int Suffix { get; set; }
        public int Digits { get; set; }
        public int Company { get; set; }
    }
}
