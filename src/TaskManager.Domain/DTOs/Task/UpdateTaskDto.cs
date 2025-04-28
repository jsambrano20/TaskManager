using TaskManager.Domain.Enums;

namespace TaskManager.Domain.DTOs
{
    public class UpdateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskManagerStatus? Status { get; set; }
    }
}
