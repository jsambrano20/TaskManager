using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskRepository
    {
        void Add(TaskItem task);
        TaskItem GetById(long id);
        List<TaskItem> GetByProjectId(long projectId);
        void Update(TaskItem task);
        void Delete(TaskItem task, long userId);
        Dictionary<string, int> GetCompletedTasksByUserInLast30Days();

    }
}
