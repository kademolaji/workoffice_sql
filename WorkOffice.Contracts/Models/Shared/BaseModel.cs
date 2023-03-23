using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WorkOffice.Contracts.Models
{
    public class BaseModel
    {
        [JsonIgnore]
        public long ClientId { get; set; }
    }
}
