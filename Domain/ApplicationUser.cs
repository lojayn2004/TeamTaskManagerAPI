using Microsoft.AspNetCore.Identity;

namespace TeamTaskManager.Domain
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }

    }
}
