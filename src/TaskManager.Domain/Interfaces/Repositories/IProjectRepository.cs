using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface IProjectRepository
    {
        List<Project> GetAll();
        void Add(Project project);
        Project GetById(long id);
        void Update(Project project);
        void Delete(Project project, long userId);

    }
}
