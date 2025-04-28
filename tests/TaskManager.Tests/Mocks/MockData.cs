
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Extensions;

namespace TaskManager.Tests.Mocks
{
    public static class MockData
    {
        public static Project GetMockProject()
        {
            var user = new User { Id = 1, Name = "João Silva" };

            var project = new Project
            {
                Id = 1,
                Name = "Projeto Exemplo",
                CreatedByUser = user,
                Tasks = new List<TaskItem>
                {
                    new TaskItem
                    {
                        Id = 1,
                        Title = "Tarefa 1",
                        Description = "Descrição da Tarefa 1",
                        DueDate = DateTime.Now.AddDays(7),
                        Priority = TaskManagerPriority.High,
                        Status = TaskManagerStatus.Pending,
                        CreatedByUser = user,
                        Comments = new List<Comment>
                        {
                            new Comment("Comentário 1", 1)
                                       {
                                CreatedByUser = user
                            },
                            new Comment("Comentário 2", 1)           {
                                CreatedByUser = user
                            }
                        }
                    },
                    new TaskItem
                    {
                        Id = 2,
                        Title = "Tarefa 2",
                        Description = "Descrição da Tarefa 2",
                        DueDate = DateTime.Now.AddDays(14),
                        Priority = TaskManagerPriority.Medium,
                        Status = TaskManagerStatus.InProgress,
                        CreatedByUser = user,
                        Comments = new List<Comment>
                        {
                            new Comment("Comentário 3", 2)
                            {
                                CreatedByUser = user
                            }
                        }
                    }
                }
            };

            return project;
        }

        public static ProjectDto GetMockProjectDto()
        {
            var project = GetMockProject();
            return project.ToDto();
        }

        public static TaskItemDto GetMockTaskItemDto()
        {
            var taskItem = GetMockProject().Tasks.First();
            return taskItem.ToDto();
        }
    }
}
