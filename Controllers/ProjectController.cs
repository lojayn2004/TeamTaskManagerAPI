using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Dtos.Project;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Controllers
{
  
    [Authorize(Policy = "ManagerOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController(IProjectService _projectService): ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateProjectAsync(CreateProjectDto createProjectDto)
        {
            var projectResult = await _projectService.AddProjectAsync(createProjectDto);
            return GetActionResult<ProjectDto>(projectResult);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectByIdAsync(Guid projectId)
        {
            var projectResult = await _projectService.GetProjectByIdAsync(projectId);
            return GetActionResult<ProjectDto?>(projectResult);
        }


        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProjectAsync( Guid projectId)
        { 
            var projectResult = await _projectService.DeleteProjectAsync(projectId);
            return GetActionResult<string>(projectResult);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateProjectAsync(UpdateProjectDto project)
        {
            var projectResult = await _projectService.UpdateProjectAsync(project);
            return GetActionResult<ProjectDto?>(projectResult);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjectsAsync()
        {
            var projectResult = await _projectService.GetAllProjectsAsync();
            return GetActionResult<IEnumerable<ProjectDto?>>(projectResult);

        }
    }
}
