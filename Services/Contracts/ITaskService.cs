using TeamTaskManager.Dtos.Project;
using TeamTaskManager.Dtos.Result;
using TeamTaskManager.Dtos.Tasks;

namespace TeamTaskManager.Services.ServicesAbstractions
{
    public interface ITaskService
    {
        public Task<ServiceResult<TaskItemDto?>> GetTaskByIdAsync(Guid taskId);

        public Task<ServiceResult<TaskItemDto>> AddTask(CreateTaskDto project);

        public Task<ServiceResult<string>> DeleteTask(Guid taskId);

        public Task<ServiceResult<TaskItemDto?>> UpdateTask(TaskItemDto task);

        public Task<ServiceResult<IEnumerable<TaskItemDto>>> GetAllTasksAsync();
    }
}
