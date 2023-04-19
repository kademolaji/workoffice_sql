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
        public long CustomIdentityFormatSettingId { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public int Digits { get; set; }
        public string Company { get; set; }
        public string Separator { get; set; }
        public string Activity { get; set; }
    }
}
