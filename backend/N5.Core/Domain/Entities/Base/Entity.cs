using N5.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace N5.Core.Domain.Entities.Base
{
    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public abstract class Entity : IEntity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}