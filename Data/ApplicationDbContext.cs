using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamTaskManager.Data.Configurations;
using TeamTaskManager.Data.Specifications;
using TeamTaskManager.Domain;

namespace TeamTaskManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().ToTable("Role");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");

            builder.ApplyConfiguration(new ProjectConfigurations());
            builder.ApplyConfiguration(new TaskConfigurations());
            builder.ApplyConfiguration(new NotificationConfiguration());
           
        }
        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<TaskItem> TaskItems { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
