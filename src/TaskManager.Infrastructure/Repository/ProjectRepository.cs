using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskManagerDbContext _context;

        public ProjectRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public List<Project> GetAll()
        {
            return _context.Projects
                .Include(p => p.CreatedByUser)
                .Include(p => p.Tasks.Where(t => t.DeletedAt == null))
                    .ThenInclude(x => x.Comments)
                        .ThenInclude(x => x.CreatedByUser)
                .ToList();
        }

        public void Add(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
        }

        public Project GetById(long id)
        {
            return _context.Projects
                .Include(p => p.Tasks.Where(t => t.DeletedAt == null))
                    .ThenInclude(x => x.Comments)
                    .ThenInclude(x => x.CreatedByUser)
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id && p.DeletedAt == null);
        }

        public void Update(Project project)
        {
            _context.Projects.Update(project);
            _context.SaveChanges();
        }

        public void Delete(Project project, long userId)
        {
            project.DeletedAt = DateTime.UtcNow;
            project.DeletedBy = userId;

            _context.Projects.Update(project);
            _context.SaveChanges();
        }
    }
}
