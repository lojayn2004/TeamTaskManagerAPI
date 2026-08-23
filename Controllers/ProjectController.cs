using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Project;
using TeamTaskManager.Dtos.Result;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Controllers
{
    //[Authorize(Roles = "Manager")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController(IProjectService _projectService): ApiBaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)
        {
            var projectResult = await _projectService.AddProject(createProjectDto);
            return GetActionResult<ProjectDto>(projectResult);
        }

        [HttpGet("projectId")]
        public async Task<IActionResult> GetProjectByIdAsync(Guid projectId)
        {
            var projectResult = await _projectService.GetProjectByIdAsync(projectId);
            return GetActionResult<ProjectDto?>(projectResult);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var projectResult = await _projectService.DeleteProject(projectId);
            return GetActionResult<string>(projectResult);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject(UpdateProjectDto project)
        {
            var projectResult = await _projectService.UpdateProject(project);
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
