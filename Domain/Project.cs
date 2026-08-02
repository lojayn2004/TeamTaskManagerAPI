namespace TeamTaskManager.Domain
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public ApplicationUser? CreatedBy {  get; set; }


        public string? CreatedByUserId { get; set; } = string.Empty;


    }
}
