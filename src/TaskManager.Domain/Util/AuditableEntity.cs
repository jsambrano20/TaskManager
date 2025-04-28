
namespace TaskManager.Domain.Util
{
    public abstract class AuditableEntity : Entity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

    }
}
