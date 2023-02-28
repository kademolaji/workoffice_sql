using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Common.Settings
{
    public class UserActivitiesList
    {
        public Guid CreateStructureDefinition { get; set; } = Guid.Parse("");
        public Guid ViewStructureDefinition { get; set; } = Guid.Parse("");
        public Guid DeleteStructureDefinition { get; set; } = Guid.Parse("");
        
    }
}
