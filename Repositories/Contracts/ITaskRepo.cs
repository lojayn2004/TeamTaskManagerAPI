using TeamTaskManager.Domain;

namespace TeamTaskManager.Repositories.Contracts
{
    public interface ITaskRepo
    {
        public Task<TaskItem?> GetTaskByIdAsync(Guid taskId);

        public Task<bool> AddTaskAsync(TaskItem taskItem);

        public bool DeleteProject(TaskItem taskItem);

        public bool UpdateProject(TaskItem taskItem);

        public Task<IEnumerable<TaskItem>> GetAllProjectsAsync();
    }
}
