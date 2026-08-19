namespace TeamTaskManager.Dtos.Tasks
{
    public class AssignTaskDto
    {
        public string AssignedUserId { get; set; } = string.Empty;

        public Guid TaskId { get; set; }
    }
}
