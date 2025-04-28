using Microsoft.AspNetCore.Http;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Extensions;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Interfaces.Services;
using TaskManager.Domain.Manager;
using TaskManager.Domain.Util;

namespace TaskManager.Application.Services.ProjectService
{
    public class ProjectAppService : IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager _userManager;

        public ProjectAppService(IProjectRepository projectRepository, UserManager userManager)
        {
            _projectRepository = projectRepository;
            _userManager = userManager;
        }

        public void CreateProject(CreateProjectDto dto)
        {
            var project = ProjectExtensions.Create(dto.Name);
            _projectRepository.Add(project);
        }

        public List<ProjectDto> GetAllProjects()
        {
            return _projectRepository.GetAll()
                .Where(p => p.DeletedAt == null)
                .Select(x => x.ToDto())
                .ToList();
        }

        public List<ProjectDto> GetAllProjectsUserLogged()
        {
            var userLogado = _userManager.GetCurrentUserId();

            if (!userLogado.HasValue)
                throw new DomainException("Usuário não autenticado.");

            return _projectRepository.GetAll()
                .Where(p => p.DeletedAt == null && p.CreatedBy == userLogado)
                .Select(x => x.ToDto())
                .ToList();
        }

        public void DeleteProject(int projectId)
        {
            var project = _projectRepository.GetById(projectId);
            if (project == null)
                throw new DomainException("Projeto não encontrado.");

            var userLogado = _userManager.GetCurrentUserId();
            if (!userLogado.HasValue)
                throw new DomainException("Usuário não autenticado.");

            project.Delete();

            _projectRepository.Delete(project, userLogado.Value);
        }
    }
}
