using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Dtos.Auth;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService _authService) : ApiBaseController
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.Login(loginDto);
            return GetActionResult(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);
            return GetActionResult(result);
        }
    }


}
