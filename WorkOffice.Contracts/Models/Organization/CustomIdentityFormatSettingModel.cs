using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class CustomIdentityFormatSettingModel : BaseModel
    {
        public Guid CustomIdentityFormatSettingId { get; set; }
        public int Prefix { get; set; }
        public int Suffix { get; set; }
        public int Digits { get; set; }
        public int Company { get; set; }
    }
}
