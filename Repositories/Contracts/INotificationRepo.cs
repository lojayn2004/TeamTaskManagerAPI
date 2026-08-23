using TeamTaskManager.Domain;

namespace TeamTaskManager.Repositories.Contracts
{
    public interface INotificationRepo
    {
        public Task<bool> AddNotificationAsync(Notification notification);
    }
}
