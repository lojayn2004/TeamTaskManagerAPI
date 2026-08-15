using TeamTaskManager.Domain;

namespace TeamTaskManager.Repositories.Contracts
{
    public interface IProjectRepo
    {
         public Task<Project?> GetProjectByIdAsync(Guid projectId);

         public Task<bool> AddProjectAsync(Project project);

         public bool DeleteProject(Project project);

         public bool UpdateProject(Project project);

         public Task<IEnumerable<Project>> GetAllProjectsAsync();
    }
}
