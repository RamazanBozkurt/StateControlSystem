using System.ComponentModel.DataAnnotations;

namespace StateControlSystem.Entities
{
    public class EntityBase : IEntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}