using TeamTaskManager.Domain;

namespace TeamTaskManager.Specfications
{
    public class UserTasksSpecification : BaseSpecification<TaskItem>
    {
        public UserTasksSpecification(string userId): 
            base(t => (t.AssignedUserId == userId))
        {

        }
    }
}
