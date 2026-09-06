using TeamTaskManager.Data.Seeding;

namespace TeamTaskManager
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedDBAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            await RolesSeeding.SeedRolesAsync(scope.ServiceProvider);
            return app;
        }
    }
}
