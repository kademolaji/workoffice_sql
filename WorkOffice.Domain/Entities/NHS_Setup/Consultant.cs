﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WorkOffice.Domain.Entities
{
    public class Consultant : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ConsultantId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
