using AutoMapper;
using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Tasks;

namespace TeamTaskManager.MappingProfiles
{
    public class TaskProfile: Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskDto, TaskItem>();

            CreateMap<TaskItem, TaskItemDto>();
        }
    }
}
