

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Dtos.Tasks;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Controllers
{
    [Authorize(Roles = "Manager")]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController(ITaskService _taskService) : ApiBaseController
    {
        [HttpPost]
        public Task<IActionResult> CreateTask(CreateTaskDto createTaskDto)
        {
            //if(!ModelState.IsValid) 
            //    return BadRequest();

            //var projectResult = _projectService.AddProject(createProjectDto);

            //return Task.FromResult("");
            throw new NotImplementedException();
        }
        [HttpPut]
        public Task<IActionResult> UpdateTask(TaskItemDto taskItemDto)
        {
            throw new NotImplementedException();
        }


        [HttpDelete]
        public Task<IActionResult> DeleteTask(Guid taskId)
        {
            throw new NotImplementedException();
        }
        [HttpPost("assign")]
        public Task<IActionResult> AssignTaskToUser(AssignTaskDto assignTaskDto)
        {
            throw new NotImplementedException();
        }

        // need authorization manager
        [HttpGet]
        public Task<IActionResult> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        [HttpGet("id")]
        public Task<IActionResult> GetTaskById(Guid taskId)
        {
            throw new NotImplementedException();
        }
    }


}
