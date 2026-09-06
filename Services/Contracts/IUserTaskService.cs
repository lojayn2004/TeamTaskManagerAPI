using TeamTaskManager.Dtos.Result;
using TeamTaskManager.Dtos.Tasks;

namespace TeamTaskManager.Services.Contracts
{
    public interface IUserTaskService
    {
        public Task<ServiceResult<TaskItemDto>> AssignTaskToUserAsync(AssignTaskDto taskDto);


        public Task<ServiceResult<TaskItemDto>> MarkTaskAsDoneAsync(Guid taskId, string UserId);

        public Task<ServiceResult<IEnumerable<TaskItemDto>>> GetUserTasksAsync(string UserId);


    }
}
