using TeamTaskManager.Dtos.Project;
using TeamTaskManager.Dtos.Result;
using TeamTaskManager.Dtos.Tasks;

namespace TeamTaskManager.Services.ServicesAbstractions
{
    public interface ITaskService
    {
        public Task<ServiceResult<TaskItemDto?>> GetTaskByIdAsync(Guid taskId);

        public Task<ServiceResult<TaskItemDto>> AddTaskAsync(CreateTaskDto project);

        public Task<ServiceResult<string>> DeleteTaskAsync(Guid taskId);

        public Task<ServiceResult<TaskItemDto?>> UpdateTaskAsync(UpdateTaskDto task);

        public Task<ServiceResult<IEnumerable<TaskItemDto>>> GetAllTasksAsync();

       
    }

}
