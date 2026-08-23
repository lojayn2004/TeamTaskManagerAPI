using Microsoft.EntityFrameworkCore;
using TeamTaskManager.Data;
using TeamTaskManager.Domain;
using TeamTaskManager.Repositories.Contracts;
using TeamTaskManager.Specfications;

namespace TeamTaskManager.Repositories.Implementations
{
    public class TaskRepo(ApplicationDbContext _dbContext) : ITaskRepo
    {
        public async Task<bool> AddTaskAsync(TaskItem taskItem)
        {
            await _dbContext.TaskItems.AddAsync(taskItem);
            return _dbContext.SaveChanges() > 0;
        }

        public bool DeleteTask(TaskItem taskItem)
        {
            _dbContext.TaskItems.Remove(taskItem);
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(ISpecification<TaskItem> spec = null)
        {
            var query = spec == null ? _dbContext.TaskItems : 
                QueryEvaluator.GetQuery(_dbContext.TaskItems, spec);
            return await query.ToListAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(Guid taskId)
        {
            return await  _dbContext.TaskItems.FindAsync(taskId);  
        }

        public async Task<TaskItem?> GetTaskByIdAsync(ISpecification<TaskItem> spec = null)
        {
            var query = spec == null ? _dbContext.TaskItems :
                QueryEvaluator.GetQuery(_dbContext.TaskItems, spec);
            return await query.FirstOrDefaultAsync();
        }

        public bool UpdateTask(TaskItem taskItem)
        {
            _dbContext.TaskItems.Update(taskItem);
            return _dbContext.SaveChanges() > 0;
        }

      
    }
}
