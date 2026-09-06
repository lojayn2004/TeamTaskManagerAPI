using System.ComponentModel.DataAnnotations.Schema;

namespace TeamTaskManager.Domain
{
    public class TaskItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public TaskStatus TaskStatus { get; set; } = TaskStatus.Pending;

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public string? AssignedUserId { get; set; } = string.Empty;

        [ForeignKey(nameof(AssignedUserId))]
        public ApplicationUser? User { get; set; }
    }
}
