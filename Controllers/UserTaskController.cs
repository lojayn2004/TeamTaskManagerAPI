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
        public async Task<IActionResult> MarkTaskAsDone(Guid taskId)
        {
            var markAsDoneResult = await _userTaskService.MarkTaskAsDone(taskId, UserId!);
            return GetActionResult<TaskItemDto>(markAsDoneResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTasks()
        {
            var userTasks = await  _userTaskService.GetUserTasks(UserId!);
            return GetActionResult<IEnumerable<TaskItemDto>>(userTasks);

        }
    }
}
