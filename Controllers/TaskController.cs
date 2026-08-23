using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Dtos.Tasks;
using TeamTaskManager.Services.Contracts;
using TeamTaskManager.Services.Implementations;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Controllers
{
    [Authorize(Roles = "Manager")]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController(ITaskService _taskService, IUserTaskService _userTaskService) : ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDto createTaskDto)
        {
            //if(!ModelState.IsValid) 
            //    return BadRequest();

            var taskResult = await _taskService.AddTask(createTaskDto);
            return GetActionResult<TaskItemDto>(taskResult);
         
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTask(TaskItemDto taskItemDto)
        {
            var taskResult = await _taskService.UpdateTask(taskItemDto);
            return GetActionResult<TaskItemDto>(taskResult);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            var taskResult = await _taskService.DeleteTask(taskId);
            return GetActionResult<string>(taskResult);
        }
        [HttpPost("assign")]
        public async Task<IActionResult> AssignTaskToUser(AssignTaskDto assignTaskDto)
        {
            var taskResult = await _userTaskService.AssignTaskToUser(assignTaskDto);
            return GetActionResult<TaskItemDto>(taskResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasksResult = await _taskService.GetAllTasksAsync();
            return GetActionResult<IEnumerable<TaskItemDto>>(tasksResult);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetTaskById(Guid taskId)
        {
            var taskResult = await _taskService.GetTaskByIdAsync(taskId);
            return GetActionResult<TaskItemDto?>(taskResult);
        }
    }


}
