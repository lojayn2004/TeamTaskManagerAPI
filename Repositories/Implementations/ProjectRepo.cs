using Microsoft.EntityFrameworkCore;
using TeamTaskManager.Data;
using TeamTaskManager.Domain;
using TeamTaskManager.Repositories.Contracts;

namespace TeamTaskManager.Repositories.Implementations
{
    public class ProjectRepo(ApplicationDbContext _dbContext) : IProjectRepo
    {
        public async Task<bool> AddProjectAsync(Project project)
        {
            
            await _dbContext.Projects.AddAsync(project);
            return _dbContext.SaveChanges() > 0;
        }

        public bool DeleteProject(Project project)
        {
            _dbContext.Projects.Remove(project);
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _dbContext.Projects.ToListAsync();

        }

        public async Task<Project?> GetProjectByIdAsync(Guid projectId)
        {
            return await _dbContext.Projects.FindAsync(projectId);
        }

        public bool UpdateProject(Project project)
        {
            _dbContext.Projects.Update(project);
            return _dbContext.SaveChanges() > 0;
        }

       
    }
}
