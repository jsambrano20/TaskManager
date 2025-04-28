
using System.ComponentModel;

namespace TaskManager.Domain.Enums
{
    public enum TaskManagerPriority
    {
        [Description("Baixa")]
        Low,   

        [Description("Média")]
        Medium,

        [Description("Alta")]
        High  
    }
}
