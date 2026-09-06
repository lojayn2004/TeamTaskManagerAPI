using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Dtos.Tasks;
using TeamTaskManager.Services.Contracts;

namespace TeamTaskManager.Controllers
{

    [Authorize(Policy = "EmployeeOnly")]
    [ApiController]
    [Route("api/user-tasks")]
    public class UserTaskController(IUserTaskService _userTaskService): ApiBaseController
    {
        [HttpPut("mark")]
        public async Task<IActionResult> MarkTaskAsDoneAsync(Guid taskId)
        {
            var markAsDoneResult = await _userTaskService.MarkTaskAsDoneAsync(taskId, UserId!);
            return GetActionResult<TaskItemDto>(markAsDoneResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTasksAsync()
        {
            var userTasks = await  _userTaskService.GetUserTasksAsync(UserId!);
            return GetActionResult<IEnumerable<TaskItemDto>>(userTasks);

        }
    }
}
