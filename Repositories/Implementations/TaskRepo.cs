using Microsoft.EntityFrameworkCore;
using TeamTaskManager.Data;
using TeamTaskManager.Domain;
using TeamTaskManager.Repositories.Contracts;

namespace TeamTaskManager.Repositories.Implementations
{
    public class TaskRepo(ApplicationDbContext _dbContext) : ITaskRepo
    {
        public async Task<bool> AddTaskAsync(TaskItem taskItem)
        {
            await _dbContext.TaskItems.AddAsync(taskItem);
            return _dbContext.SaveChanges() > 0;
        }

        public bool DeleteProject(TaskItem taskItem)
        {
            _dbContext.TaskItems.Remove(taskItem);
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<IEnumerable<TaskItem>> GetAllProjectsAsync()
        {
            return await _dbContext.TaskItems.ToListAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(Guid taskId)
        {
            return await  _dbContext.TaskItems.FindAsync(taskId);
           
        }

        public bool UpdateProject(TaskItem taskItem)
        {
            _dbContext.TaskItems.Update(taskItem);
            return _dbContext.SaveChanges() > 0;
        }
    }
}
