namespace TeamTaskManager.Dtos.Tasks
{
    public class MarkTaskDoneDto
    {
        public Guid ProjectId { get; set; }

        public Guid TaskId { get; set; }
    }
}
