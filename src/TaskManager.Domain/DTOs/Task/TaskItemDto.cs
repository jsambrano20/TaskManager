
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.DTOs
{
    public class TaskItemDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public long ProjectId { get; set; }
        public List<TaskHistory> History { get; set; } = new();
        public List<CommentDto> Comments { get; set; } = new();
        public string Author { get; set; }

    }
}
