using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Application.Services.TaskService;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Interfaces.Services;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectAppService _projectAppService;

        public ProjectsController(IProjectAppService projectAppService)
        {
            _projectAppService = projectAppService;
        }


        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo projeto.")]
        public IActionResult CreateProject([FromBody] CreateProjectDto dto)
        {
            _projectAppService.CreateProject(dto);
            return Ok("Projeto criado com sucesso.");
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Recupera todos os projetos existentes.")]
        public IActionResult GetAllProjects()
        {
            var projects = _projectAppService.GetAllProjects();
            return Ok(projects);
        }

        [HttpGet("UserLogged")]
        [SwaggerOperation(Summary = "Recupera todos os projetos do usuário logado.")]
        public IActionResult GetAllProjectsUserLogged()
        {
            var projects = _projectAppService.GetAllProjectsUserLogged();
            return Ok(projects);
        }

        [HttpDelete("{projectId}")]
        [SwaggerOperation(Summary = "Deleta um projeto pelo ID.")]
        public IActionResult DeleteProject(int projectId)
        {
            _projectAppService.DeleteProject(projectId);
            return Ok("Projeto removido com sucesso.");
        }
    }
}
