using TeamTaskManager.Dtos;

namespace TeamTaskManager.Services.ServicesAbstractions
{
    public interface IAuthService
    {
        public Task<AuthResponseDto> Login(LoginDto loginDto);

        public Task<AuthResponseDto> Register(RegisterDto registerDto);
    }
}
