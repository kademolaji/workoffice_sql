using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public partial class State : Entity
    {

        public string Name { get; set; }
        public string Code { get; set; }
        [ForeignKey("Country")]
        public long CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
