using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Manager;

namespace TaskManager.Tests.Services
{
    public class StubUserManager : UserManager
    {
        private readonly int? _currentUserId;

        public StubUserManager(int? currentUserId)
            : base(new StubHttpContextAccessor())
        {
            _currentUserId = currentUserId;
        }

        public override long? GetCurrentUserId()
        {
            return _currentUserId;
        }
    }

    // Implementação stub para IHttpContextAccessor
    public class StubHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; } = new DefaultHttpContext();
    }
}
