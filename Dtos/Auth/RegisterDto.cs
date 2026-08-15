namespace TeamTaskManager.Dtos.Auth
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; } // TODO: NEED TO CHANGE TO ENUM

        public string Password { get; set; } = string.Empty;
    }
}
