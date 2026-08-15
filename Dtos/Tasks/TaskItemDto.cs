using TeamTaskManager.Domain;

namespace TeamTaskManager.Dtos.Tasks
{
    public class TaskItemDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string TaskStatus { get; set; }

        public Guid ProjectId { get; set; }

        public string? AssignedUserId { get; set; } = string.Empty;

       
    }
}
