
using System.ComponentModel;

namespace TaskManager.Domain.Enums
{
    public enum UserRole
    {
        [Description("User")]
        User,   

        [Description("Gerente")]
        Manager,

        [Description("Admin")]
        Admin
    }
}
