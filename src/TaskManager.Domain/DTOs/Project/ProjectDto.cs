namespace TaskManager.Domain.DTOs
{
    public class ProjectDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public List<TaskItemDto> Tasks { get; set; } = new();
    }
}
