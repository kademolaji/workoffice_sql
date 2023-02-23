using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WorkOffice.Domain.Entities.Shared;

namespace WorkOffice.Domain.Entities.NHS_Setup 
{
    public class Activity : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ActivityId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
