using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeamTaskManager.Data;
using TeamTaskManager.Domain;
using TeamTaskManager.Repositories.Contracts;
using TeamTaskManager.Repositories.Implementations;


namespace TeamTaskManager.Extensions
{
    public static class DbServicesRegisterationExtension
    {
        public static WebApplicationBuilder DbServiceRegister(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


       
            builder.Services.AddScoped<IProjectRepo, ProjectRepo>();
            builder.Services.AddScoped<ITaskRepo, TaskRepo>();
            builder.Services.AddScoped<INotificationRepo, NotificationRepo>();

            return builder;
        }
    }
}
