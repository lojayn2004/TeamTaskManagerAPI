using AutoMapper;
using System.Security.Claims;
using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Project;

namespace TeamTaskManager.MappingProfiles
{
    public class UserIdResolver(IHttpContextAccessor _httpContextAccessor) : IValueResolver<CreateProjectDto, Project, string?>
    {

        public string? Resolve(CreateProjectDto source, 
            Project destination, 
            string? destMember, 
            ResolutionContext context)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
