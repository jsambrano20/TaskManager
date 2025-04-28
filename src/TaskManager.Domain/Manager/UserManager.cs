using Microsoft.AspNetCore.Http;
using TaskManager.Domain.Interfaces;
namespace TaskManager.Domain.Manager
{
    //queria usar esse metodo sem precisar ficar passando os metodos do construtor para ele, metodo global 
    public class UserManager 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual long? GetCurrentUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var userIdStr = httpContext.Items["UserId"]?.ToString();

                if (!string.IsNullOrEmpty(userIdStr) && long.TryParse(userIdStr, out var userId))
                {
                    return userId;
                }
            }
            return null;
        }
    }
}
