﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace WorkOffice.Domain.Entities.NHS_Setup 
{
    public class RTT : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid RTTId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
