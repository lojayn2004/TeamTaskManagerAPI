using AutoMapper;
using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Result;
using TeamTaskManager.Dtos.Tasks;
using TeamTaskManager.Repositories.Contracts;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Services.Implementations
{
    public class TaskService(ITaskRepo _taskRepo, IProjectRepo _projectRepo,  IMapper _mapper): ITaskService
    {
        public async Task<ServiceResult<TaskItemDto>> AddTask(CreateTaskDto createTaskDto)
        {
            var task = _mapper.Map<TaskItem>(createTaskDto);
            var project = await _projectRepo.GetProjectByIdAsync(createTaskDto.ProjectId);
            if (project == null)
                return ServiceResult<TaskItemDto>.Error(ServiceError.NotFound, $"Cannot Add Task, Project with Id {createTaskDto.ProjectId} is Not Found");

            await _taskRepo.AddTaskAsync(task);
            var taskDto = _mapper.Map<TaskItemDto>(task);
            return ServiceResult<TaskItemDto>.Ok(taskDto);
        }

        public async Task<ServiceResult<string>> DeleteTask(Guid taskId)
        {
            var task = await _taskRepo.GetTaskByIdAsync(taskId);
            if (task == null)
                return ServiceResult<string>.Error(ServiceError.NotFound, $"Task with Id {taskId} is Not Found");

            _taskRepo.DeleteTask(task);
            return ServiceResult<string>.Ok($"Task with Id {taskId} is Deleted Successfully");
        }

        public async Task<ServiceResult<IEnumerable<TaskItemDto>>> GetAllTasksAsync()
        {
            var tasks = await _taskRepo.GetAllTasksAsync();
            var mappedTasks = _mapper.Map<IEnumerable<TaskItemDto>>(tasks);
            return ServiceResult<IEnumerable<TaskItemDto>>.Ok(mappedTasks);
        }

        public async Task<ServiceResult<TaskItemDto?>> GetTaskByIdAsync(Guid taskId)
        {
            var task = await _taskRepo.GetTaskByIdAsync(taskId);
            if (task == null)
                return ServiceResult<TaskItemDto?>.Error(ServiceError.NotFound, $"Task with Id {taskId} is Not Found");

            var mappedTask = _mapper.Map<TaskItemDto>(task);
            return ServiceResult<TaskItemDto?>.Ok(mappedTask);
        }

        public async Task<ServiceResult<TaskItemDto?>> UpdateTask(TaskItemDto taskDto)
        {
            var task = await _taskRepo.GetTaskByIdAsync(taskDto.Id);
            if (task == null)
                return ServiceResult<TaskItemDto?>.Error(ServiceError.NotFound, $"Task with Id {taskDto.Id} is Not Found");

            _mapper.Map(taskDto, task);
            _taskRepo.UpdateTask(task);

            var mappedTask = _mapper.Map<TaskItemDto>(taskDto);
            return ServiceResult<TaskItemDto?>.Ok(mappedTask);
        }
    }
}
