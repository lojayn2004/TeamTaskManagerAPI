using TeamTaskManager.Dtos;
using TeamTaskManager.Repositories.Contracts;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Services.Implementations
{
    public class ProjectService(IProjectRepo _projectRepo) : IProjectService
    {
        public void AddProject(ProjectDto project)
        {
            throw new NotImplementedException();
        }

        public void DeleteProject(ProjectDto project)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto?> GetProjectByIdAsync(string projectId)
        {
            throw new NotImplementedException();
        }

        public void UpdateProject(ProjectDto project)
        {
            throw new NotImplementedException();
        }
    }
}
