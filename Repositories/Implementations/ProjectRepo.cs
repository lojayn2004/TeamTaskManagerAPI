using Microsoft.EntityFrameworkCore;
using TeamTaskManager.Data;
using TeamTaskManager.Domain;
using TeamTaskManager.Repositories.Contracts;

namespace TeamTaskManager.Repositories.Implementations
{
    public class ProjectRepo(ApplicationDbContext _dbContext) : IProjectRepo
    {
        public void AddProject(Project project)
        {
            _dbContext.Projects.Add(project);
            _dbContext.SaveChanges();
        }

        public void DeleteProject(Project project)
        {
            _dbContext.Projects.Remove(project);
            _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _dbContext.Projects.ToListAsync();

        }

        public async Task<Project?> GetProjectByIdAsync(string projectId)
        {
            return await _dbContext.Projects.FindAsync(projectId);
        }

        public void UpdateProject(Project project)
        {
            _dbContext.Projects.Update(project);
            _dbContext.SaveChanges();
        }
    }
}
