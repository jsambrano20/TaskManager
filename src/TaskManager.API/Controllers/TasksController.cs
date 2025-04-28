using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Manager,Admin")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskAppService _taskAppService;

        public TasksController(ITaskAppService taskAppService)
        {
            _taskAppService = taskAppService;
        }

        [HttpPost("{projectId}")]
        [SwaggerOperation(Summary = "Cria uma nova tarefa associada ao projeto.")]
        public IActionResult CreateTask(int projectId, [FromBody] AddTaskDto dto)
        {
            _taskAppService.CreateTask(projectId, dto);
            return Ok("Tarefa criada com sucesso.");
        }

        [HttpPut("{taskId}")]
        [SwaggerOperation(Summary = "Atualiza uma tarefa existente.")]
        public IActionResult UpdateTask(int taskId, [FromBody] UpdateTaskDto dto)
        {
            _taskAppService.UpdateTask(taskId, dto);
            return Ok("Tarefa atualizada com sucesso.");
        }

        [HttpDelete("{taskId}")]
        [SwaggerOperation(Summary = "Remove uma tarefa pelo ID.")]
        public IActionResult DeleteTask(int taskId)
        {
            _taskAppService.DeleteTask(taskId);
            return Ok("Tarefa removida com sucesso.");
        }

        [HttpGet("{projectId}")]
        [SwaggerOperation(Summary = "Recupera todas as tarefas associadas a um projeto.")]
        public IActionResult GetAllTaskByProjectId(int projectId)
        {
            var tasks = _taskAppService.GetByProjectId(projectId);
            return Ok(tasks);
        }

        [HttpPost("{taskId}/comments")]
        [SwaggerOperation(Summary = "Adiciona um comentário a uma tarefa existente.")]
        public IActionResult AddCommentToTask(int taskId, [FromBody] AddCommentDto dto)
        {
            _taskAppService.AddCommentToTask(taskId, dto);
            return Ok("Comentário adicionado à tarefa com sucesso.");
        }
    }
}
