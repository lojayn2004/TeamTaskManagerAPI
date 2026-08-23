using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Auth;
using TeamTaskManager.Dtos.Result;
using TeamTaskManager.Services.ServicesAbstractions;
using TeamTaskManager.Utils;

namespace TeamTaskManager.Services.Implementations
{
    public class AuthService(UserManager<ApplicationUser> _userManager, 
         IOptions<JwtConfigurations> _jwtOptions): IAuthService
    {
        public async Task<ServiceResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return ServiceResult<AuthResponseDto>.Error(ServiceError.UnAuthorized, "Incorrect Email Or Password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return ServiceResult<AuthResponseDto>.Error(ServiceError.UnAuthorized, "Incorrect Email Or Password");

            var authDto =  new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = await JwtHelper.GenerateJwtToken(user, _userManager, _jwtOptions)
            };
            return ServiceResult<AuthResponseDto>.Ok(authDto);
        }

        public async Task<ServiceResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
                return ServiceResult<AuthResponseDto>.Error(ServiceError.Conflict, "Email Already Registered");
            var applicationUser = new ApplicationUser()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FullName = registerDto.FullName
            };
            var result = await _userManager.CreateAsync(applicationUser, registerDto.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
                return null;
                
            }
            var roleResult = await _userManager.AddToRoleAsync(applicationUser, registerDto.Role);
            if(!roleResult.Succeeded)
            {
                return null;
            }


            var authDto =  new AuthResponseDto
            {
                UserId = applicationUser.Id,
                Email = applicationUser.Email,
                Token = await JwtHelper.GenerateJwtToken(applicationUser, _userManager, _jwtOptions)
            };
            return ServiceResult<AuthResponseDto>.Ok(authDto);
        }

     
    }
}
