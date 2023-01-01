using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkOffice.Domain.Entities.Shared
{
    public class Entity<TPrimaryKey>
    {
        public Entity()
        {
            IsDeleted = false;
            CreatedOn = DateTimeOffset.Now;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TPrimaryKey Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public bool IsDeleted { get; set; }
    }
}
