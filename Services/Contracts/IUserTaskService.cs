using TeamTaskManager.Dtos.Result;
using TeamTaskManager.Dtos.Tasks;

namespace TeamTaskManager.Services.Contracts
{
    public interface IUserTaskService
    {
        public Task<ServiceResult<TaskItemDto>> AssignTaskToUser(AssignTaskDto taskDto);


        public Task<ServiceResult<TaskItemDto>> MarkTaskAsDone(Guid taskId, string UserId);

        public Task<ServiceResult<IEnumerable<TaskItemDto>>> GetUserTasks(string UserId);


    }
}
