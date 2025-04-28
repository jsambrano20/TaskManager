
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Tests.Services.TasksTest
{
    public class StubTaskRepository : ITaskRepository
    {
        private readonly List<TaskItem> _tasks = new();

        public void Add(TaskItem task)
        {
            _tasks.Add(task);
        }

        public TaskItem GetById(long id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public List<TaskItem> GetByProjectId(long projectId)
        {
            return _tasks.Where(t => t.ProjectId == projectId).ToList();
        }

        public void Update(TaskItem task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.Status = task.Status;
                existingTask.Comments = task.Comments;
                existingTask.History = task.History;
            }
        }

        public void Delete(TaskItem task, long userId)
        {
            _tasks.Remove(task);
        }

        public Dictionary<string, int> GetCompletedTasksByUserInLast30Days()
        {
            throw new NotImplementedException();
        }
    }
}

