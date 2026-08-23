using TeamTaskManager.Domain;
using TeamTaskManager.Specfications;

namespace TeamTaskManager.Repositories.Contracts
{
    public interface ITaskRepo
    {
        public Task<TaskItem?> GetTaskByIdAsync(Guid taskId);
        public Task<TaskItem?> GetTaskByIdAsync(ISpecification<TaskItem> spec = null);

        public Task<bool> AddTaskAsync(TaskItem taskItem);

        public bool DeleteTask(TaskItem taskItem);

        public bool UpdateTask(TaskItem taskItem);

        public Task<IEnumerable<TaskItem>> GetAllTasksAsync(ISpecification<TaskItem> spec = null);
    }
}
