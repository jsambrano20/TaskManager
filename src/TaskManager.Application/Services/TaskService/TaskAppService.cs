using Microsoft.AspNetCore.Http;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Extensions;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Interfaces.Services;
using TaskManager.Domain.Manager;
using TaskManager.Domain.Util;

namespace TaskManager.Application.Services.TaskService
{
    public class TaskAppService : ITaskAppService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager _userManager;

        public TaskAppService(ITaskRepository taskRepository, IProjectRepository projectRepository, UserManager userManager)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _userManager = userManager;
        }

        public void CreateTask(long projectId, AddTaskDto dto)
        {
            var project = _projectRepository.GetById(projectId);
            if (project == null)
                throw new DomainException("Projeto não encontrado.");

            if (project.Tasks.Count >= 20)
                throw new DomainException("O projeto já atingiu o limite máximo de 20 tarefas.");

            var task = TaskItemExtensions.Create(dto.Title, dto.Description, dto.DueDate, dto.Priority);
            project.Tasks.Add(task);

            _projectRepository.Update(project);
        }

        public List<TaskItemDto> GetByProjectId(long projectId)
        {
            var project = _projectRepository.GetById(projectId);

            if (project == null)
                throw new DomainException("Projeto não encontrado.");

            var task = _taskRepository.GetByProjectId(projectId).Select(x => x.ToDto()).ToList();

            if (task == null)
                throw new DomainException("Nenhuma Tarefa encontrada para o projeto informado.");

            return task;
        }

        public void UpdateTask(long taskId, UpdateTaskDto dto)
        {
            var task = _taskRepository.GetById(taskId);
            if (task == null)
                throw new DomainException("Tarefa não encontrada.");

            task.UpdateFromDto(dto);

            _taskRepository.Update(task);
        }

        public void DeleteTask(long taskId)
        {
            var task = _taskRepository.GetById(taskId);
            if (task == null)
                throw new DomainException("Tarefa não encontrada.");

            var userLogado = _userManager.GetCurrentUserId();
            if (!userLogado.HasValue)
                throw new DomainException("Usuário não autenticado.");

            _taskRepository.Delete(task, userLogado.Value);
        }

        /// <summary>
        /// Adiciona um comentário a uma tarefa.
        /// </summary>
        public void AddCommentToTask(long taskId, AddCommentDto dto)
        {
            var task = _taskRepository.GetById(taskId);
            if (task == null)
                throw new DomainException("Tarefa não encontrada.");

            // Usa o método de extensão para adicionar o comentário
            task.AddCommentFromDto(dto);

            _taskRepository.Update(task);
        }
    }
}
