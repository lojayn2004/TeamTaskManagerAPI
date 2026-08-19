using TeamTaskManager.Domain;

namespace TeamTaskManager.Repositories.Contracts
{
    public interface ITaskRepo
    {
        public Task<TaskItem?> GetTaskByIdAsync(Guid taskId);

        public Task<bool> AddTaskAsync(TaskItem taskItem);

        public bool DeleteTask(TaskItem taskItem);

        public bool UpdateTask(TaskItem taskItem);

        public Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    }
}
