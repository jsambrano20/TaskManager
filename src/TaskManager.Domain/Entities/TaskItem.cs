using TaskManager.Domain.Enums;
using TaskManager.Domain.Util;

namespace TaskManager.Domain.Entities
{
    public class TaskItem : AuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskManagerStatus Status { get; set; }
        public TaskManagerPriority Priority { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }

        public List<TaskHistory> History { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public virtual User CreatedByUser { get; set; }

    }
}
