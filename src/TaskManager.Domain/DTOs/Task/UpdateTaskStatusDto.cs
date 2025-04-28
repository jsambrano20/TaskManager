using TaskManager.Domain.Enums;

namespace TaskManager.Domain.DTOs
{
    public class UpdateTaskStatusDto
    {
        public TaskManagerStatus Status { get; set; }
        public string UpdatedBy { get; set; }
    }
}
