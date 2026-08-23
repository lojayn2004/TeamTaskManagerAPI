using TeamTaskManager.MappingProfiles;
using TeamTaskManager.Repositories.Contracts;
using TeamTaskManager.Repositories.Implementations;
using TeamTaskManager.Services.Contracts;
using TeamTaskManager.Services.Implementations;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Extensions
{
    public static class ServiceClassesRegisterationExtension
    {
        public static WebApplicationBuilder RegisterServiceClasses(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<IUserTaskService, UserTaskService>();
            builder.Services.AddAutoMapper(cfg => { }, typeof(ProjectProfile));

            return builder;

        }
    }
}
