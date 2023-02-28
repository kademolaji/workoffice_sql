using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WorkOffice.Domain.Entities.NHS_Setup
{
    public class Consultant : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ConsultantId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
