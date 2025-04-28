using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.Services
{
    public interface ITaskAppService
    {
        void CreateTask(long projectId, AddTaskDto dto);
        List<TaskItemDto> GetByProjectId(long projectId);
        void UpdateTask(long taskId, UpdateTaskDto dto);
        void DeleteTask(long taskId);
        void AddCommentToTask(long taskId, AddCommentDto dto);
    }
}
