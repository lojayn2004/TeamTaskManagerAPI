using AutoMapper;
using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Project;

namespace TeamTaskManager.MappingProfiles
{
    public class ProjectProfile: Profile
    {
        public ProjectProfile() 
        {
            CreateMap<CreateProjectDto, Project>()
                .ForMember(p => p.CreatedAt, c => c.MapFrom(s => DateTime.Now))
                .ForMember(p => p.CreatedByUserId, c => c.MapFrom<UserIdResolver>());

            CreateMap<Project, ProjectDto>();

            CreateMap<ProjectDto, Project>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateProjectDto, Project>();


        }
    }
}
