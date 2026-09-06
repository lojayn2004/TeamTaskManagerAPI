using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Dtos.Tasks;
using TeamTaskManager.Services.Contracts;
using TeamTaskManager.Services.Implementations;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Controllers
{
    [Authorize(Policy = "ManagerOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController(ITaskService _taskService, IUserTaskService _userTaskService) : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            

            var taskResult = await _taskService.AddTaskAsync(createTaskDto);
            return GetActionResult<TaskItemDto>(taskResult);
         
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTaskAsync(UpdateTaskDto taskItemDto)
        {
            var taskResult = await _taskService.UpdateTaskAsync(taskItemDto);
            return GetActionResult<TaskItemDto>(taskResult);
        }


        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTaskAsync(Guid taskId)
        {
            var taskResult = await _taskService.DeleteTaskAsync(taskId);
            return GetActionResult<string>(taskResult);
        }
        [HttpPost("assign")]
        public async Task<IActionResult> AssignTaskToUserAsync(AssignTaskDto assignTaskDto)
        {
            var taskResult = await _userTaskService.AssignTaskToUserAsync(assignTaskDto);
            return GetActionResult<TaskItemDto>(taskResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasksAsync()
        {
            var tasksResult = await _taskService.GetAllTasksAsync();
            return GetActionResult<IEnumerable<TaskItemDto>>(tasksResult);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskByIdAsync(Guid taskId)
        {
            var taskResult = await _taskService.GetTaskByIdAsync(taskId);
            return GetActionResult<TaskItemDto?>(taskResult);
        }
    }


}
