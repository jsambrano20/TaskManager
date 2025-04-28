using Xunit;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Application.Services.ProjectService;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Manager;
using TaskManager.Tests.Mocks;

namespace TaskManager.Tests.Services.ProjectTest
{
    public class ProjectAppServiceTests
    {
        private readonly ProjectAppService _projectAppService;
        private readonly StubProjectRepository _stubProjectRepository;
        private readonly StubUserManager _stubUserManager;

        public ProjectAppServiceTests()
        {
            _stubProjectRepository = new StubProjectRepository();
            _stubUserManager = new StubUserManager(1);
            _projectAppService = new ProjectAppService(_stubProjectRepository, _stubUserManager);
        }

        [Fact]
        public void CreateProject_ShouldAddProjectToRepository()
        {
            // Arrange
            var dto = new CreateProjectDto { Name = "Novo Projeto" };

            // Act
            _projectAppService.CreateProject(dto);

            // Assert
            var projects = _stubProjectRepository.GetAll();
            Assert.Single(projects);
            Assert.Equal("Novo Projeto", projects[0].Name);
        }

        [Fact]
        public void GetAllProjectsUserLogged_ShouldReturnProjectsCreatedByUser()
        {
            // Arrange
            var mockProject1 = MockData.GetMockProject();
            mockProject1.CreatedBy = 1; // Usuário logado
            _stubProjectRepository.Add(mockProject1);

            var mockProject2 = MockData.GetMockProject();
            mockProject2.Id = 2;
            mockProject2.Name = "Projeto 2";
            mockProject2.CreatedBy = 2; // Outro usuário
            _stubProjectRepository.Add(mockProject2);

            // Act
            var result = _projectAppService.GetAllProjectsUserLogged();

            // Assert
            Assert.Single(result);
            Assert.Equal("Projeto Exemplo", result[0].Name);
        }
    }
}