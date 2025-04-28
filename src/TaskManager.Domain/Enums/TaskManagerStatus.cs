
using System.ComponentModel;

namespace TaskManager.Domain.Enums
{
    public enum TaskManagerStatus
    {
        [Description("Pendente")]
        Pending,   

        [Description("Em andamento")]
        InProgress,

        [Description("Concluída")]
        Completed  
    }
}
