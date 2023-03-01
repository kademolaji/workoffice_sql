using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models.NHS_Setup
{
    public class PathwayStatusViewModels
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool AllowClosed { get; set; }
    }
}
