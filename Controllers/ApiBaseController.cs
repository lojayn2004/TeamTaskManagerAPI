using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamTaskManager.Dtos.Result;

namespace TeamTaskManager.Controllers
{
   
    public abstract class ApiBaseController: ControllerBase
    {
        protected string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        protected string? UserName => User.FindFirst(ClaimTypes.Name)?.Value;
        protected string? UserEmail => User.FindFirst(ClaimTypes.Email)?.Value;

        protected IActionResult GetActionResult<T> (ServiceResult<T> result)
        {
            if (result.Success == true)
                return Ok(new { data = result.Data, success = true });

            if (result.ErrorType == ServiceError.UnAuthorized)
                return Unauthorized(new { message = result.Message, sucess = false });
            if (result.ErrorType == ServiceError.Validation)
                return BadRequest(new { message = result.Message, success = false });
            if (result.ErrorType == ServiceError.NotFound)
                return NotFound(new { message = result.Message, success = false });

            return Problem(detail: result.Message);

        }


    }
}
