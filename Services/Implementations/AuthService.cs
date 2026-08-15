using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Auth;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Services.Implementations
{
    public class AuthService(UserManager<ApplicationUser> _userManager, 
         IOptions<JwtConfigurations> _jwtOptions): IAuthService
    {
        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return null;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return null;

            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = await GenerateJwtToken(user)
            };
        }

        public async Task<AuthResponseDto> Register(RegisterDto registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
                return null;
            var applicationUser = new ApplicationUser()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FullName = registerDto.FullName
            };
            var result = await _userManager.CreateAsync(applicationUser, registerDto.Password);
            if(!result.Succeeded)
            {
                return null;
                
            }
            var roleResult = await _userManager.AddToRoleAsync(applicationUser, registerDto.Role);
            if(!roleResult.Succeeded)
            {
                return null;
            }


            return new AuthResponseDto
            {
                UserId = applicationUser.Id,
                Email = applicationUser.Email,
                Token = await GenerateJwtToken(applicationUser)
            };
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = await GetClaims(user);
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtOptions.Value.Key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                issuer: _jwtOptions.Value.Issuer,
                audience: _jwtOptions.Value.Audience,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddDays(7)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
