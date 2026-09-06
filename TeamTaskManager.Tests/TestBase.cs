using System.Text.Json;

namespace TeamTaskManager.Tests
{
    public abstract class TestBase
    {
        protected string BaseUrl { get; }

        protected TestBase()
        {
            // Allow overriding via environment variable (e.g. CI/CD or Docker), fallback to launchSettings URL
            BaseUrl = Environment.GetEnvironmentVariable("TEST_API_URL")?.TrimEnd('/') 
                      ?? "http://localhost:5151";
        }

        /// <summary>
        /// Generates a unique suffix to avoid collisions in database identity fields (emails, usernames).
        /// </summary>
        protected static string GetUniqueId() => Guid.NewGuid().ToString("N")[..8];

        /// <summary>
        /// Parses the JSON response body into a JsonDocument for flexible property extraction.
        /// </summary>
        protected static JsonDocument ParseJson(string responseBody)
        {
            return JsonDocument.Parse(responseBody);
        }
    }
}
