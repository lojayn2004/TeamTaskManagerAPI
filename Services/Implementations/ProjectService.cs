using AutoMapper;
using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Project;
using TeamTaskManager.Dtos.Result;
using TeamTaskManager.Repositories.Contracts;
using TeamTaskManager.Services.ServicesAbstractions;

namespace TeamTaskManager.Services.Implementations
{
    public class ProjectService(IProjectRepo _projectRepo, IMapper _mapper) : IProjectService
    {
        public async Task<ServiceResult<ProjectDto>> AddProject(CreateProjectDto createProjectDto)
        {
            var project = _mapper.Map<Project>(createProjectDto);
            bool isAddedSuccessfully = await _projectRepo.AddProjectAsync(project);
            var projectDto = _mapper.Map<ProjectDto>(project);
            return ServiceResult<ProjectDto>.Ok(projectDto);
        }

        public async Task<ServiceResult<string>> DeleteProject(Guid projectId)
        {
           var project = await  _projectRepo.GetProjectByIdAsync(projectId);

            if (project == null)
                return ServiceResult<string>.NotFound($"Project with id {projectId} is not found");
            _projectRepo.DeleteProject(project);
            return ServiceResult<string>.Ok($"Project With Id {projectId} is deleted successfully");
        }

        public async Task<ServiceResult<IEnumerable<ProjectDto>>> GetAllProjectsAsync()
        {
            var projects = await _projectRepo.GetAllProjectsAsync();
            var mappedProjects =  _mapper.Map<IEnumerable<ProjectDto>>(projects);
            return ServiceResult<IEnumerable<ProjectDto>>.Ok(mappedProjects);
        }

        public async Task<ServiceResult<ProjectDto?>> GetProjectByIdAsync(Guid projectId)
        {
            var project =  await _projectRepo.GetProjectByIdAsync(projectId);
            if (project == null)
                return ServiceResult<ProjectDto?>.NotFound($"Project with id {projectId} is not found");
            var mappedProject =  _mapper.Map<ProjectDto>(project);
            return ServiceResult<ProjectDto?>.Ok(mappedProject);
        }

        public async Task<ServiceResult<ProjectDto?>> UpdateProject(UpdateProjectDto projectDto)
        {
            var project = await _projectRepo.GetProjectByIdAsync(projectDto.Id);
            if (project == null)
                return ServiceResult<ProjectDto?>.NotFound($"Project with id {projectDto.Id} is not found");
            _mapper.Map(projectDto, project);
            _projectRepo.UpdateProject(project);
            var mappedProject = _mapper.Map<ProjectDto>(project);
            return ServiceResult<ProjectDto?>.Ok(mappedProject);
        }

        
    }
}
