using Microsoft.AspNetCore.Http;
using System.Text;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Extensions;
using TaskManager.Infrastructure;

namespace TaskManager.API.Extensions
{
    public class AuthenticationMiddlewareExtension
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddlewareExtension(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TaskManagerDbContext dbContext)
        {
            if (context.Request.Path == "/" || context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var authHeaderValue = authHeader.ToString();
                if (authHeaderValue.StartsWith("Basic "))
                {
                    var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue.Substring("Basic ".Length)));
                    var parts = credentials.Split(':');
                    if (parts.Length == 2)
                    {
                        var email = parts[0];
                        var password = parts[1];

                        var user = dbContext.Users.FirstOrDefault(u => u.Email == email);
                        if (user != null && UserExtensions.VerifyPasswordHash(password, user.Password))
                        {
                            context.Items["UserId"] = user.Id.ToString();
                            context.Items["UserRole"] = user.Role.ToString();

                            await _next(context);
                            return;
                        }
                    }
                }
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Credenciais inválidas.");
        }
    }
    }
