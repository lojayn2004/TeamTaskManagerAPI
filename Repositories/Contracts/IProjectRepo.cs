using TeamTaskManager.Domain;

namespace TeamTaskManager.Repositories.Contracts
{
    public interface IProjectRepo
    {
         public Task<Project?> GetProjectByIdAsync(string projectId);

         public void AddProject(Project project);

         public void DeleteProject(Project project);

         public void UpdateProject(Project project);

         public Task<IEnumerable<Project>> GetAllProjectsAsync();
    }
}
