using TaskManager.Domain.Enums;

namespace TaskManager.Domain.DTOs
{
    public class AddTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskManagerPriority Priority { get; set; }
    }
}
