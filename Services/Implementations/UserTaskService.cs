using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Result;
using TeamTaskManager.Dtos.Tasks;
using TeamTaskManager.Hubs;
using TeamTaskManager.Repositories.Contracts;
using TeamTaskManager.Services.Contracts;
using TeamTaskManager.Specfications;

namespace TeamTaskManager.Services.Implementations
{
    public class UserTaskService(ITaskRepo _taskRepo, 
        UserManager<ApplicationUser> _userManager,
        IMapper _mapper,
        IHubContext<NotificationHub> _hubContext,
        INotificationRepo _notificationRepo) : IUserTaskService
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

        public async Task<ServiceResult<IEnumerable<TaskItemDto>>> GetUserTasks(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return ServiceResult<IEnumerable<TaskItemDto>>.Error(ServiceError.NotFound, $"User with Id {UserId} is Not Found");
            var userTaskSpec = new UserTasksSpecification(UserId);
            var tasks =  await _taskRepo.GetAllTasksAsync(userTaskSpec);
            return ServiceResult<IEnumerable<TaskItemDto>>.Ok(_mapper.Map<IEnumerable<TaskItemDto>>(tasks));
        }

        public async Task<ServiceResult<TaskItemDto>> MarkTaskAsDone(Guid taskId, string UserId)
        {
            var task = await _taskRepo.GetTaskByIdAsync(new TaskWithProjectSpecification(taskId));
            
            if (task == null)
                return ServiceResult<TaskItemDto>.Error(ServiceError.NotFound, $"Task with Id {taskId} is Not Found");

            Console.WriteLine("===================================================================");
            Console.WriteLine(task.Project.CreatedByUserId);
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return ServiceResult<TaskItemDto>.Error(ServiceError.NotFound, $"User with Id {UserId} is Not Found");

            if (task.AssignedUserId != UserId)
                return ServiceResult<TaskItemDto>.Error(ServiceError.UnAuthorized, $"Cannot Mark Task As Done, UnAuthorized User {UserId}");
            
            task.TaskStatus = Domain.TaskStatus.Done;
            _taskRepo.UpdateTask(task);
            Console.WriteLine("===================================================================");
            Console.WriteLine(task.Project.CreatedByUserId);


            if (task.Project.CreatedByUserId != null)
            {
                string message =  $"Task {taskId} is completed by Employee {user.Email}.";
                await _hubContext.Clients.User(task.Project.CreatedByUserId)
                       .SendAsync("ReceiveTaskNotification",message);

                await _notificationRepo.AddNotificationAsync(new Notification()
                {
                    UserId = task.Project.CreatedByUserId,
                    SentAt = DateTime.Now,
                    Message = message
                });
            }



            var mappedTask = _mapper.Map<TaskItemDto>(task);
            return ServiceResult<TaskItemDto>.Ok(mappedTask);
        }
    }
}
