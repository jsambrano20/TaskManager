using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.Services
{
    public interface IProjectAppService
    {
        void CreateProject(CreateProjectDto dto);
        List<ProjectDto> GetAllProjects();
        List<ProjectDto> GetAllProjectsUserLogged();
        void DeleteProject(int projectId);
    }
}
