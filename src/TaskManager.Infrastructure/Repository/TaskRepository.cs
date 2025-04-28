using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _context;

        public TaskRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public void Add(TaskItem task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public TaskItem GetById(long id)
        {
            return _context.Tasks
                .Include(t=>t.CreatedByUser)
                .Include(t => t.History)
                .Include(t => t.Comments)
                    .ThenInclude(x => x.CreatedByUser)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(t => t.Id == id && t.DeletedAt == null);
        }

        public List<TaskItem> GetByProjectId(long projectId)
        {
            return _context.Tasks
                .Include(t => t.CreatedByUser)
                .Include(t => t.History)
                .Include(t => t.Comments)
                    .ThenInclude(x => x.CreatedByUser)
                .AsNoTrackingWithIdentityResolution()
                .Where(t => t.ProjectId == projectId && t.DeletedAt == null)
                .ToList();
        }
        public void Update(TaskItem task)
        {
            _context.Update(task);

            foreach (var comment in task.Comments)
            {
                if (comment.Id == 0)
                {
                    _context.Entry(comment).State = EntityState.Added;
                }
                else
                {
                    _context.Entry(comment).State = EntityState.Modified;
                }
            }
            foreach (var history in task.History)
            {
                if (history.Id == 0)
                {
                    _context.Entry(history).State = EntityState.Added;
                }
                else
                {
                    _context.Entry(history).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
        }

        public void Delete(TaskItem task, long userId)
        {
            task.DeletedAt = DateTime.UtcNow;
            task.DeletedBy = userId;

            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public Dictionary<string, int> GetCompletedTasksByUserInLast30Days()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            return _context.Tasks
                .Include(t => t.CreatedByUser)
                .Where(t => t.Status == TaskManagerStatus.Completed && t.UpdatedAt >= thirtyDaysAgo)
                .GroupBy(t => t.CreatedByUser.Email)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count()
                );
        }
    }
}
