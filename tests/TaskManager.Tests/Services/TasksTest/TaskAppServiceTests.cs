using Xunit;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Application.Services.TaskService;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Manager;
using TaskManager.Tests.Mocks;
using TaskManager.Tests.Services;
using TaskManager.Domain.Util;
using TaskManager.Tests.Services.ProjectTest;
using TaskManager.Tests.Services.TasksTest;
using TaskManager.Domain.Enums;

namespace TaskManager.Tests.Services.TaskTest
{
    public class TaskAppServiceTests
    {
        private readonly TaskAppService _taskAppService;
        private readonly StubTaskRepository _stubTaskRepository;
        private readonly StubProjectRepository _stubProjectRepository;
        private readonly StubUserManager _stubUserManager;

        public TaskAppServiceTests()
        {
            _stubTaskRepository = new StubTaskRepository();
            _stubProjectRepository = new StubProjectRepository();
            _stubUserManager = new StubUserManager(1);
            _taskAppService = new TaskAppService(_stubTaskRepository, _stubProjectRepository, _stubUserManager);
        }

        #region Criação de Tarefas

        [Fact]
        public void CreateTask_ShouldThrowException_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = 999;
            var dto = new AddTaskDto { Title = "Nova Tarefa" };

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => _taskAppService.CreateTask(projectId, dto));
            Assert.Equal("Projeto não encontrado.", exception.Message);
        }

        [Fact]
        public void CreateTask_ShouldThrowException_WhenTaskLimitIsReached()
        {
            // Arrange
            var mockProject = MockData.GetMockProject();
            mockProject.Tasks = Enumerable.Range(1, 20).Select(i => new TaskItem { Title = $"Tarefa {i}" }).ToList();
            _stubProjectRepository.Add(mockProject);

            var dto = new AddTaskDto { Title = "Nova Tarefa" };

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => _taskAppService.CreateTask(mockProject.Id, dto));
            Assert.Equal("O projeto já atingiu o limite máximo de 20 tarefas.", exception.Message);
        }

        #endregion

        #region Atualização de Tarefas

        [Fact]
        public void UpdateTask_ShouldUpdateTaskDetails()
        {
            // Arrange
            var mockProject = MockData.GetMockProject();
            _stubProjectRepository.Add(mockProject);

            var task = mockProject.Tasks.First();
            _stubTaskRepository.Add(task);

            var dto = new UpdateTaskDto
            {
                Title = "Tarefa Atualizada",
                Status = TaskManagerStatus.Completed
            };

            // Act
            _taskAppService.UpdateTask(task.Id, dto);

            // Assert
            var updatedTask = _stubTaskRepository.GetById(task.Id);
            Assert.Equal("Tarefa Atualizada", updatedTask.Title);
            Assert.Equal(TaskManagerStatus.Completed, updatedTask.Status);
        }

        [Fact]
        public void UpdateTask_ShouldThrowException_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = 999; // Tarefa inexistente
            var dto = new UpdateTaskDto { Title = "Tarefa Atualizada" };

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => _taskAppService.UpdateTask(taskId, dto));
            Assert.Equal("Tarefa não encontrada.", exception.Message);
        }

        #endregion

        #region Remoção de Tarefas

        [Fact]
        public void DeleteTask_ShouldRemoveTaskFromProject()
        {
            // Arrange
            var mockProject = MockData.GetMockProject();
            _stubProjectRepository.Add(mockProject);

            var task = mockProject.Tasks.First();
            _stubTaskRepository.Add(task);

            // Act
            _taskAppService.DeleteTask(task.Id);

            // Assert
            var tasks = _stubTaskRepository.GetByProjectId(mockProject.Id);
            Assert.Empty(tasks);
        }

        [Fact]
        public void DeleteTask_ShouldThrowException_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = 999; // Tarefa inexistente

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => _taskAppService.DeleteTask(taskId));
            Assert.Equal("Tarefa não encontrada.", exception.Message);
        }

        #endregion

        #region Comentários nas Tarefas

        [Fact]
        public void AddCommentToTask_ShouldAddCommentAndRegisterInHistory()
        {
            // Arrange
            var mockProject = MockData.GetMockProject();
            _stubProjectRepository.Add(mockProject);

            var task = new TaskItem
            {
                Id = 1,
                Title = "Tarefa Limpa",
                ProjectId = mockProject.Id,
                Comments = new List<Comment>() // Garante que a tarefa não tenha comentários iniciais
            };
            _stubTaskRepository.Add(task);

            var commentDto = new AddCommentDto { Text = "Comentário de teste" };

            // Act
            _taskAppService.AddCommentToTask(task.Id, commentDto);

            // Assert
            var updatedTask = _stubTaskRepository.GetById(task.Id);
            Assert.Single(updatedTask.Comments); // Deve haver apenas 1 comentário
            Assert.Contains(updatedTask.Comments, c => c.Text == "Comentário de teste");

            Assert.Single(updatedTask.History); // Deve haver 1 entrada no histórico
            Assert.Contains(updatedTask.History, h => h.ChangeDescription.Contains("Comentário adicionado"));
        }
        #endregion
    }
}