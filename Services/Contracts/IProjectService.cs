using TeamTaskManager.Domain;
using TeamTaskManager.Dtos;

namespace TeamTaskManager.Services.ServicesAbstractions
{
    public interface IProjectService
    {
        public Task<ProjectDto?> GetProjectByIdAsync(string projectId);

        public void AddProject(ProjectDto project);

        public void DeleteProject(ProjectDto project);

        public void UpdateProject(ProjectDto project);

        public Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    }
}
