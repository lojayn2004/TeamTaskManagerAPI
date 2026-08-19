using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Result;
using TeamTaskManager.Dtos.Tasks;
using TeamTaskManager.Repositories.Contracts;
using TeamTaskManager.Services.Contracts;

namespace TeamTaskManager.Services.Implementations
{
    public class UserTaskService(ITaskRepo _taskRepo, 
        UserManager<ApplicationUser> _userManager, IMapper _mapper): IUserTaskService
    {
        public async Task<ServiceResult<TaskItemDto>> AssignTaskToUser(AssignTaskDto taskDto)
        {
            var task = await _taskRepo.GetTaskByIdAsync(taskDto.TaskId);
            if (task == null)
                return ServiceResult<TaskItemDto>.Error(ServiceError.NotFound, $"Task with Id {taskDto.TaskId} is Not Found");

            var user = await _userManager.FindByIdAsync(taskDto.AssignedUserId);
            if (user == null)
                return ServiceResult<TaskItemDto>.Error(ServiceError.NotFound, $"User with Id {taskDto.AssignedUserId} is Not Found");

            task.AssignedUserId = taskDto.AssignedUserId;
            task.User = user;

            task.TaskStatus = Domain.TaskStatus.InProgress;
            _taskRepo.UpdateTask(task);
            var mappedTask = _mapper.Map<TaskItemDto>(task);
            return ServiceResult<TaskItemDto>.Ok(mappedTask);
        }

        public Task<ServiceResult<IEnumerable<TaskItemDto>>> GetUserTasks(string UserId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<TaskItemDto>> MarkTaskAsDone(Guid taskId, string UserId)
        {
            var task = await _taskRepo.GetTaskByIdAsync(taskId);
            if (task == null)
                return ServiceResult<TaskItemDto>.Error(ServiceError.NotFound, $"Task with Id {taskId} is Not Found");

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return ServiceResult<TaskItemDto>.Error(ServiceError.NotFound, $"User with Id {UserId} is Not Found");

            if (task.AssignedUserId != UserId)
                return ServiceResult<TaskItemDto>.Error(ServiceError.UnAuthorized, $"Cannot Mark Task As Done, UnAuthorized User {UserId}");
            
            task.TaskStatus = Domain.TaskStatus.Done;

            _taskRepo.UpdateTask(task);
            var mappedTask = _mapper.Map<TaskItemDto>(task);
            return ServiceResult<TaskItemDto>.Ok(mappedTask);
        }
    }
}
