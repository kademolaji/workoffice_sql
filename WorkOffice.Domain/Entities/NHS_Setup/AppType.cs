using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace WorkOffice.Domain.Entities
{
    public class AppType : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long AppTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
