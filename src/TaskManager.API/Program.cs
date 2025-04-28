using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TaskManager.API.Extensions;
using TaskManager.Domain.Util;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Adiciona o filtro de exce��es globalmente
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

// Configurar Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManager API", Version = "v1" });
    options.DescribeAllParametersInCamelCase();

    options.AddSecurityDefinition("BasicAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Insira seu email e senha para autentica��o."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BasicAuth" }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddEndpointsApiExplorer();

// Adicionar autentica��o e especificar o handler customizado para "Basic" Auth
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

// Adicionar autoriza��o
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
});

var app = builder.Build();

// Aplicar migra��es automaticamente (opcional)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();
    dbContext.Database.Migrate();

    // Executa o seed de usu�rios
    SeedUser.Initialize(dbContext);
}

app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AuthenticationMiddlewareExtension>();

app.MapControllers();

app.Run();
