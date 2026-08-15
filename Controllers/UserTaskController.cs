using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Dtos.Tasks;

namespace TeamTaskManager.Controllers
{
    
    [ApiController]
    [Route("api/user-tasks")]
    public class UserTaskController: ApiBaseController
    {
        // mark task as done

        [HttpPut("mark")]
        public Task<IActionResult> MarkTaskAsDone(MarkTaskDoneDto markTaskDoneDto)
        {
            throw new NotImplementedException();
        }


        // get all my tasks 

        [HttpGet]
        public Task<IActionResult> GetUserTasks()
        {
            throw new NotImplementedException();
        }
    }
}
