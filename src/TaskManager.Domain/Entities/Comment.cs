using TaskManager.Domain.Util;

namespace TaskManager.Domain.Entities
{
    public class Comment : AuditableEntity
    {
        public string Text { get; set; }
        public long TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }
        public virtual User CreatedByUser { get; set; }
        public Comment(string text, long taskItemId)
        {
            Text = text;
            TaskItemId = taskItemId;
        }
    }
}
