using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Domain.Entities.Shared
{
    public class Country : Entity<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsAfrica { get; set; }
    }
}
