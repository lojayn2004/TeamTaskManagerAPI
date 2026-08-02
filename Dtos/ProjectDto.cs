

namespace TeamTaskManager.Dtos
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string CreatedByUserId { get; set; } = string.Empty;

    }
}
