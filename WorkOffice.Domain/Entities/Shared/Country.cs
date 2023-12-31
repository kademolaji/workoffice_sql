﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class Country : Entity
    {
        [Key]
        public long CountryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsAfrica { get; set; }
    }
}
