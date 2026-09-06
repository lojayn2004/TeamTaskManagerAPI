using TeamTaskManager.Domain;

namespace TeamTaskManager.Specfications
{
    public class TaskWithProjectSpecification: BaseSpecification<TaskItem>
    {
        public TaskWithProjectSpecification(Guid taskId) : base(t => t.Id ==  taskId) 
        {
            Console.WriteLine("Size Before: " + IncludeClause.Count);
            AddInclude(t => t.Project);
            Console.WriteLine("Size After: " + IncludeClause.Count);
        }
    }
}
