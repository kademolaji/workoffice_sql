using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Common.Settings
{
    public class UserActivitiesList
    {
        public Guid CreateStructureDefinition { get; set; } = Guid.Parse("1072E43D-2A19-4950-8878-87BA717CFF16");
        public Guid ViewStructureDefinition { get; set; } = Guid.Parse("96BAB20B-188C-47B7-9C05-5DFAFA4C2CF3");
        public Guid DeleteStructureDefinition { get; set; } = Guid.Parse("A8FC1FB0-3CAD-4B36-BC94-BBF403142C1B");
        
    }
}
