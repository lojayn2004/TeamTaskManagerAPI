using System.ComponentModel.DataAnnotations;

namespace TeamTaskManager.Dtos.Project
{
    public class CreateProjectDto
    {
        
        [Required]
        public string Name { get; set; } = default!;

        [Required]

        public string Description { get; set; } = default!;

    }


}
