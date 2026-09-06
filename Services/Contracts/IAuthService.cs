using TeamTaskManager.Dtos.Auth;
using TeamTaskManager.Dtos.Result;

namespace TeamTaskManager.Services.ServicesAbstractions
{
    public interface IAuthService
    {
        public Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto);

        public Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
    }
}
