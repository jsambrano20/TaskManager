using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Services.ProjectService;
using TaskManager.Application.Services.TaskService;
using TaskManager.Application.Services.ReportService;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Interfaces.Services;
using TaskManager.Domain.Manager;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Repository;

namespace TaskManager.API.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();  
            services.AddScoped<UserManager>();
            services.AddScoped<IProjectAppService, ProjectAppService>();
            services.AddScoped<ITaskAppService, TaskAppService>();
            services.AddScoped<IReportAppService, ReportAppService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<TaskManagerDbContext>((serviceProvider, options) =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                var userId = httpContextAccessor.HttpContext?.User.Identity.Name;


                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                       .EnableSensitiveDataLogging(); 

                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            return services;
        }
    }
}
