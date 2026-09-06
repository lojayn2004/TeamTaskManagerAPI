using Microsoft.AspNetCore.Identity;

namespace TeamTaskManager.Domain
{
    public class ApplicationUser: IdentityUser

    {
        public string FullName { get; set; }


        public List<Notification> Notifications { get; set; }

        public List<TaskItem> Tasks { get; set; }

    }
}
