using System.ComponentModel.DataAnnotations;

namespace TeamTaskManager.Dtos.Project
{
    public class UpdateProjectDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string Description { get; set; } = default!;
    }
}
