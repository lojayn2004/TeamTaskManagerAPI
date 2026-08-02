namespace TeamTaskManager.Domain
{
    public class TaskItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public TaskStatus TaskStatus { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public string? AssignedUserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }
    }
}
