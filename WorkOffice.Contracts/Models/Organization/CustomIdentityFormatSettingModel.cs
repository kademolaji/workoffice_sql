using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class CustomIdentityFormatSettingModel : BaseModel
    {
        public long CustomIdentityFormatSettingId { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public int Digits { get; set; }
        public string Company { get; set; }
    }
}
