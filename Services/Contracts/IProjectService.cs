using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Project;
using TeamTaskManager.Dtos.Result;

namespace TeamTaskManager.Services.ServicesAbstractions
{
    public interface IProjectService
    {
        public Task<ServiceResult<ProjectDto?>> GetProjectByIdAsync(Guid projectId);

        public Task<ServiceResult<ProjectDto>> AddProject(CreateProjectDto project);

        public Task<ServiceResult<string>> DeleteProject(Guid projectId);

        public Task<ServiceResult<ProjectDto?>> UpdateProject(UpdateProjectDto project);

        public Task<ServiceResult<IEnumerable<ProjectDto>>> GetAllProjectsAsync();
    }
}
