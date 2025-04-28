using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Util;

namespace TaskManager.Domain.Extensions
{
    public static class TaskItemExtensions
    {
        public static TaskItem Create(string title, string description, DateTime dueDate, TaskManagerPriority priority)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("O título da tarefa é obrigatório.");

            return new TaskItem
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                Priority = priority,
                Status = TaskManagerStatus.Pending
            };
        }

        public static TaskItemDto ToDto(this TaskItem taskItem)
        {
            if (taskItem == null) return null;

            return new TaskItemDto
            {
                Id = taskItem.Id,
                Author = taskItem.CreatedByUser.Name,
                Title = taskItem.Title,
                Description = taskItem.Description,
                DueDate = taskItem.DueDate,
                Priority = EnumHelper.GetDescription(taskItem.Priority),
                Status = EnumHelper.GetDescription(taskItem.Status),
                ProjectId = taskItem.ProjectId,
                Comments = taskItem.Comments.Select(c => new CommentDto
                {
                    Text = c.Text,
                    Author = c.CreatedByUser.Name

                }).ToList()
            };
        }

        public static void UpdateStatus(this TaskItem task, TaskManagerStatus newStatus)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (task.Status == newStatus)
                return;

            task.Status = newStatus;
            task.AddHistory($"Status alterado para {newStatus}");
        }

        public static void AddComment(this TaskItem task, string text)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            var comment = new Comment(text, task.Id);
            task.Comments.Add(comment);
            task.AddHistory($"Comentário adicionado: {text}");
        }

        public static void UpdateTitle(this TaskItem task, string newTitle)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new DomainException("O título da tarefa é obrigatório.");

            task.Title = newTitle;
            task.AddHistory($"Título alterado para {newTitle}");
        }

        public static void UpdateDescription(this TaskItem task, string newDescription)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            task.Description = newDescription;
            task.AddHistory($"Descrição alterada para {newDescription}");
        }

        public static void UpdateFromDto(this TaskItem task, UpdateTaskDto dto)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            if (!string.IsNullOrEmpty(dto.Title))
                task.UpdateTitle(dto.Title);

            if (!string.IsNullOrEmpty(dto.Description))
                task.UpdateDescription(dto.Description);

            if (dto.Status.HasValue)
                task.UpdateStatus(dto.Status.Value);
        }

        public static void AddCommentFromDto(this TaskItem task, AddCommentDto dto)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            task.AddComment(dto.Text);
        }

        private static void AddHistory(this TaskItem task, string changeDescription)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            task.History.Add(new TaskHistory(changeDescription, task.Id));
        }
    }
}
