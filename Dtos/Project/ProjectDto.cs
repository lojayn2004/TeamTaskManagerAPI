using System.ComponentModel.DataAnnotations;

namespace TeamTaskManager.Dtos.Project
{
    public class ProjectDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public string CreatedByUserId { get; set; } = string.Empty;

    }
}
