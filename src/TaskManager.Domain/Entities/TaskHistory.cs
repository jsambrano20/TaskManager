using TaskManager.Domain.Util;

namespace TaskManager.Domain.Entities
{
    public class TaskHistory : AuditableEntity
    {
        public string ChangeDescription { get; set; }
        public long TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }

        public TaskHistory(string changeDescription, long taskItemId)
        {
            ChangeDescription = changeDescription;
            TaskItemId = taskItemId;
        }
    }
}
