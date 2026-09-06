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

        /// <summary>
        /// Registers and logs in a user with the specified role, returning their token, user ID, and email.
        /// </summary>
        protected (string Token, string UserId, string Email) RegisterAndLogin(string role, string? customUniqueId = null)
        {
            var uniqueId = customUniqueId ?? GetUniqueId();
            var email = $"{role.ToLower()}_{uniqueId}@taskmanager.com";
            var password = "SecurePassword123!";
            var userName = $"{role.ToLower()}_{uniqueId}";
            var fullName = $"{role} User {uniqueId}";

            // 1. Register
            RestAssured.Dsl.Given()
                .ContentType("application/json")
                .Body(new
                {
                    userName,
                    fullName,
                    email,
                    role,
                    password
                })
            .When()
                .Post($"{BaseUrl}/api/auth/register")
            .Then()
                .StatusCode(200);

            // 2. Login
            var loginResponse = RestAssured.Dsl.Given()
                .ContentType("application/json")
                .Body(new
                {
                    email,
                    password
                })
            .When()
                .Post($"{BaseUrl}/api/auth/login")
            .Then()
                .StatusCode(200)
                .Extract().Body();

            using var doc = ParseJson(loginResponse);
            var token = doc.RootElement.GetProperty("data").GetProperty("token").GetString()!;
            var userId = doc.RootElement.GetProperty("data").GetProperty("userId").GetString()!;

            return (token, userId, email);
        }

        /// <summary>
        /// Creates a project using a Manager token and returns the generated project ID.
        /// </summary>
        protected string CreateProject(string managerToken, string? projectName = null)
        {
            var name = projectName ?? $"Project_{GetUniqueId()}";
            var response = RestAssured.Dsl.Given()
                .ContentType("application/json")
                .OAuth2(managerToken)
                .Body(new
                {
                    name,
                    description = "Test Project Description"
                })
            .When()
                .Post($"{BaseUrl}/api/project")
            .Then()
                .StatusCode(200)
                .Extract().Body();

            using var doc = ParseJson(response);
            return doc.RootElement.GetProperty("data").GetProperty("id").GetString()!;
        }

        /// <summary>
        /// Creates a task under a project using a Manager token and returns the generated task ID.
        /// </summary>
        protected string CreateTask(string managerToken, string projectId, string? taskTitle = null)
        {
            var title = taskTitle ?? $"Task_{GetUniqueId()}";
            var response = RestAssured.Dsl.Given()
                .ContentType("application/json")
                .OAuth2(managerToken)
                .Body(new
                {
                    title,
                    description = "Test Task Description",
                    projectId = Guid.Parse(projectId)
                })
            .When()
                .Post($"{BaseUrl}/api/task")
            .Then()
                .StatusCode(200)
                .Extract().Body();

            using var doc = ParseJson(response);
            return doc.RootElement.GetProperty("data").GetProperty("id").GetString()!;
        }

        /// <summary>
        /// Assigns a task to an employee using a Manager token.
        /// </summary>
        protected void AssignTask(string managerToken, string taskId, string employeeUserId)
        {
            RestAssured.Dsl.Given()
                .ContentType("application/json")
                .OAuth2(managerToken)
                .Body(new
                {
                    assignedUserId = employeeUserId,
                    taskId = Guid.Parse(taskId)
                })
            .When()
                .Post($"{BaseUrl}/api/task/assign")
            .Then()
                .StatusCode(200);
        }
    }
}
