using System;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Extensions;

namespace TaskManager.Infrastructure.Seed
{
    public class SeedUser
    {
        public static void Initialize(TaskManagerDbContext context)
        {
            // Lista de usuários iniciais com roles
            var users = new List<User>
        {
            new User
            {
                Name = "Administrador",
                Email = "admin@example.com",
                Password = "Admin@123".GeneratePasswordHash(),
                Role = UserRole.Admin
            },
            new User
            {
                Name = "João Silva",
                Email = "joao.silva@example.com",
                Password = "Senha123".GeneratePasswordHash(),
                Role = UserRole.Manager
            },
            new User
            {
                Name = "Maria Souza",
                Email = "maria.souza@example.com",
                Password = "Senha456".GeneratePasswordHash(),
                Role = UserRole.User 
            }
        };

            foreach (var user in users)
            {
                var existingUser = context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    existingUser.Role = user.Role;
                    Console.WriteLine($"Usuário atualizado: {user.Email}");
                }
                else
                {
                    context.Users.Add(user);
                    Console.WriteLine($"Novo usuário adicionado: {user.Email}");
                }
            }

            context.SaveChanges();

            Console.WriteLine("Seed de usuários concluído com sucesso.");
        }
    }
}
