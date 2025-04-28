using TaskManager.Domain.Enums;
using TaskManager.Domain.Util;

namespace TaskManager.Domain.Entities
{
    public class User : AuditableEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }
}
