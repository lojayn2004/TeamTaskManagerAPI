using TeamTaskManager.Domain;
using TeamTaskManager.Dtos;

namespace TeamTaskManager.Mappers
{
    public static class ProjectMapper
    {
        public static Project Map(ProjectDto projectDto)
        {
            return new Project
            { 
                Id = projectDto.Id,
                Name = projectDto.Name,
                CreatedAt = projectDto.CreatedAt,
                CreatedByUserId = projectDto.CreatedByUserId
            };

        }
    }
}
