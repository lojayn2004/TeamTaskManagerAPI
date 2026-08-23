using TeamTaskManager.Domain;

namespace TeamTaskManager.Specfications
{
    public class TaskWithProjectSpecification: BaseSpecification<TaskItem>
    {
        public TaskWithProjectSpecification(Guid taskId) : base(t => t.Id ==  taskId) 
        {
            AddInclude(t => t.Project);
        }
    }
}
