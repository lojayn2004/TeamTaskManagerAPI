using TeamTaskManager.Dtos.Auth;
using TeamTaskManager.Dtos.Result;

namespace TeamTaskManager.Services.ServicesAbstractions
{
    public interface IAuthService
    {
        public Task<ServiceResult<AuthResponseDto>> Login(LoginDto loginDto);

        public Task<ServiceResult<AuthResponseDto>> Register(RegisterDto registerDto);
    }
}
