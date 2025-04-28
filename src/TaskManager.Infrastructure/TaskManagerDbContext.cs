using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Util;

namespace TaskManager.Infrastructure
{
    public class TaskManagerDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override int SaveChanges()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var userIdStr = httpContext.Items["UserId"]?.ToString();
                if (string.IsNullOrEmpty(userIdStr))
                    throw new InvalidOperationException("Usuário não autenticado. Operação não permitida.");

                var userId = long.Parse(userIdStr);

                if (!ChangeTracker.HasChanges())
                    return 0;

                var entries = ChangeTracker.Entries();

                foreach (var entry in entries)
                {
                    if (entry.Entity is AuditableEntity auditableEntity)
                    {
                        var now = DateTime.UtcNow;

                        switch (entry.State)
                        {
                            case EntityState.Added:
                                auditableEntity.CreatedAt = now;
                                auditableEntity.CreatedBy = userId;
                                break;

                            case EntityState.Modified:
                                auditableEntity.UpdatedAt = now;
                                auditableEntity.UpdatedBy = userId;
                                break;

                            case EntityState.Deleted:
                                auditableEntity.DeletedAt = now;
                                auditableEntity.DeletedBy = userId;
                                entry.State = EntityState.Modified;
                                break;
                        }
                    }
                }
            }

            return base.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(ConfigureProject);
            modelBuilder.Entity<TaskItem>(ConfigureTaskItem);
            modelBuilder.Entity<TaskHistory>(ConfigureTaskHistory);
            modelBuilder.Entity<Comment>(ConfigureComment);
            modelBuilder.Entity<User>(ConfigureUser);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Name).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Role).HasConversion<string>().IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.CreatedAt).IsRequired();
            builder.Property(u => u.UpdatedAt).IsRequired(false);
        }

        private void ConfigureProject(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);


            builder.HasOne(c => c.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(c => c.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Tasks)
                   .WithOne(t => t.Project)
                   .HasForeignKey(t => t.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureTaskItem(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("Tasks");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
            builder.Property(t => t.Description).HasMaxLength(1000);
            builder.Property(t => t.DueDate).IsRequired();
            builder.Property(t => t.Status).HasConversion<string>().IsRequired();
            builder.Property(t => t.Priority).HasConversion<string>().IsRequired();

            builder.HasOne(c => c.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(c => c.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.History)
                   .WithOne(h => h.TaskItem)
                   .HasForeignKey(h => h.TaskItemId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Comments)
                   .WithOne(c => c.TaskItem)
                   .HasForeignKey(c => c.TaskItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureTaskHistory(EntityTypeBuilder<TaskHistory> builder)
        {
            builder.ToTable("TaskHistories");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired(); 

            builder.Property(h => h.ChangeDescription).IsRequired().HasMaxLength(500);
            builder.Property(h => h.CreatedAt).IsRequired();

            builder.HasOne(h => h.TaskItem)
                   .WithMany(t => t.History)
                   .HasForeignKey(h => h.TaskItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureComment(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Text).IsRequired().HasMaxLength(1000);
            builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(100);
            builder.Property(c => c.CreatedAt).IsRequired();

            builder.HasOne(c => c.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(c => c.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.TaskItem)
                   .WithMany(t => t.Comments)
                   .HasForeignKey(c => c.TaskItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
