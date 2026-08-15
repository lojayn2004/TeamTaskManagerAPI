using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManager.Dtos.Project;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Controllers
{
    [Authorize(Roles = "Manager")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController(IProjectService _projectService): ApiBaseController
    {
        [HttpPost]
        public Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)
        {
            //if(!ModelState.IsValid) 
            //    return BadRequest();

            //var projectResult = _projectService.AddProject(createProjectDto);

            //return Task.FromResult("");
            throw new NotImplementedException();
        }
    }
}
