namespace TeamTaskManager.Dtos
{
    public class JwtConfigurations
    {
        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;
    }
}
