using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public partial class State : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long StateId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        [ForeignKey("CountryId")]
        public long CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
