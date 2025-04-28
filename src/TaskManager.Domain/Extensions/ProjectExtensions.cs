using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Manager;
using TaskManager.Domain.Util;

namespace TaskManager.Domain.Extensions
{
    public static class ProjectExtensions
    {
        public static Project Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("O nome do projeto é obrigatório.");

            return new Project
            {
                Name = name
            };
        }

        public static ProjectDto ToDto(this Project project)
        {
            if (project == null) return null;

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Author = project.CreatedByUser.Name,
                Tasks = project.Tasks.Select(t => new TaskItemDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Priority = EnumHelper.GetDescription(t.Priority),
                    Status = EnumHelper.GetDescription(t.Status),
                    ProjectId = project.Id,
                    Comments = t.Comments.Select(c=> new CommentDto
                    {
                        Text = c.Text,
                        Author = c.CreatedByUser.Name

                    }).ToList()
                    
                }).ToList()
            };
        }

        public static void AddTask(this Project project, TaskItem task)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (task == null) throw new ArgumentNullException(nameof(task));

            if (project.Tasks.Count >= 20)
                throw new DomainException("O projeto já atingiu o limite máximo de 20 tarefas.");

            project.Tasks.Add(task);
        }

        public static void Delete(this Project project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));

            if (project.Tasks.Any(t => t.Status == TaskManagerStatus.Pending))
                throw new DomainException("Não é possível remover um projeto com tarefas pendentes.");

        }

        public static void UpdateName(this Project project, string newName)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("O nome do projeto é obrigatório.");

            project.Name = newName;
        }
    }
}
