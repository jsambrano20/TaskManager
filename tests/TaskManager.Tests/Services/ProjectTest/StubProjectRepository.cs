
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Tests.Services.ProjectTest
{
    public class StubProjectRepository : IProjectRepository
    {
        private readonly List<Project> _projects = new();

        public List<Project> GetAll()
        {
            return _projects.Where(p => p.DeletedAt == null).ToList();
        }

        public void Add(Project project)
        {
            if (_projects.Any(p => p.Id == project.Id))
                throw new InvalidOperationException("Projeto com ID duplicado.");

            _projects.Add(project);
        }

        public Project GetById(long id)
        {
            return _projects.FirstOrDefault(p => p.Id == id && p.DeletedAt == null);
        }

        public void Update(Project project)
        {
            var existingProject = _projects.FirstOrDefault(p => p.Id == project.Id);
            if (existingProject == null)
                throw new InvalidOperationException("Projeto não encontrado.");

            existingProject.Name = project.Name;
            existingProject.UpdatedAt = DateTime.Now;
        }

        public void Delete(Project project, long userId)
        {
            var existingProject = _projects.FirstOrDefault(p => p.Id == project.Id);
            if (existingProject == null)
                throw new InvalidOperationException("Projeto não encontrado.");

            existingProject.DeletedAt = DateTime.Now;
            existingProject.DeletedBy = userId;
        }
    }
}

