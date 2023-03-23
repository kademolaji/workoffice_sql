using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class StructureDefinitionModel 
    {
        public long StructureDefinitionId { get; set; }
        public string Definition { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public long ClientId { get; set; }
    }
}
