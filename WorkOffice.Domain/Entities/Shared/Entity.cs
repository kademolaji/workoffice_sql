using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkOffice.Domain.Entities
{
    public class Entity
    {
        public Entity()
        {
            IsDeleted = false;
            CreatedOn = DateTimeOffset.Now;
        }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public Guid ClientId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
