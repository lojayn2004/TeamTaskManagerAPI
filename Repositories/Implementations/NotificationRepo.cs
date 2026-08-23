using TeamTaskManager.Data;
using TeamTaskManager.Domain;
using TeamTaskManager.Repositories.Contracts;

namespace TeamTaskManager.Repositories.Implementations
{
    public class NotificationRepo(ApplicationDbContext _context) : INotificationRepo
    {
        public async Task<bool> AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            return _context.SaveChanges() > 0;
           
        }
    }
}
