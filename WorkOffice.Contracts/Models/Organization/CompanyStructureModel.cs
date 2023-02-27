using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WorkOffice.Contracts.Models
{
    public class CompanyStructureModel
    {
        public Guid CompanyStructureId { get; set; }
        public string Name { get; set; }
        public Guid StructureTypeId { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string CompanyHead { get; set; }
        public Guid? ParentID { get; set; }
        public string Parent { get; set; }
        [JsonIgnore]
        public string Company { get; set; }
        public Guid ClientId { get; set; }
    }
}
