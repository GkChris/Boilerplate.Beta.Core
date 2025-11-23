using System.ComponentModel.DataAnnotations;

namespace Boilerplate.Beta.Core.Application.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
