﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Contracts.Models
{
    public class PathwayStatusViewModels
    {
        public Guid PathwayStatusId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool AllowClosed { get; set; }
    }
}
