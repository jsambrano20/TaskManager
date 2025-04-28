using TaskManager.Domain.Enums;
using TaskManager.Domain.Util;

namespace TaskManager.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public string Name { get; set; }
        public List<TaskItem> Tasks { get; set; } = new();
        public virtual User CreatedByUser { get; set; }

    }
}
