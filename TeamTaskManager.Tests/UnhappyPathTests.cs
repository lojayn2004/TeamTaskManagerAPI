using Xunit;
using static RestAssured.Dsl;
using NHamcrest;

namespace TeamTaskManager.Tests
{
    /// <summary>
    /// Integration tests for unhappy paths:
    /// - Authentication failures (invalid credentials, unregistered users, duplicate registration)
    /// - Resource Not Found handling (invalid project, task, or user IDs)
    /// </summary>
    public class UnhappyPathTests : TestBase
    {
        // =========================================================================
        // 1. Authentication & Registration Unhappy Paths
        // =========================================================================

        [Fact]
        public void Login_WithIncorrectPassword_Returns401Unauthorized()
        {
            var uniqueId = GetUniqueId();
            var email = $"user_{uniqueId}@taskmanager.com";
            var validPassword = "SecurePassword123!";

            // Register user first
            Given()
                .ContentType("application/json")
                .Body(new
                {
                    userName = $"user_{uniqueId}",
                    fullName = "Test User",
                    email,
                    role = "Employee",
                    password = validPassword
                })
            .When()
                .Post($"{BaseUrl}/api/auth/register")
            .Then()
                .StatusCode(200);

            // Attempt login with wrong password
            Given()
                .ContentType("application/json")
                .Body(new
                {
                    email,
                    password = "WrongPassword999!"
                })
            .When()
                .Post($"{BaseUrl}/api/auth/login")
            .Then()
                .StatusCode(401)
                .Body("message", NHamcrest.Is.EqualTo("Incorrect Email Or Password"));
        }

        [Fact]
        public void Login_WithNonExistentEmail_Returns401Unauthorized()
        {
            var uniqueId = GetUniqueId();
            var nonExistentEmail = $"ghost_{uniqueId}@taskmanager.com";

            Given()
                .ContentType("application/json")
                .Body(new
                {
                    email = nonExistentEmail,
                    password = "AnyPassword123!"
                })
            .When()
                .Post($"{BaseUrl}/api/auth/login")
            .Then()
                .StatusCode(401)
                .Body("message", NHamcrest.Is.EqualTo("Incorrect Email Or Password"));
        }

        [Fact]
        public void Register_WithDuplicateEmail_ReturnsError()
        {
            var uniqueId = GetUniqueId();
            var email = $"duplicate_{uniqueId}@taskmanager.com";
            var password = "SecurePassword123!";

            // First registration succeeds
            Given()
                .ContentType("application/json")
                .Body(new
                {
                    userName = $"user1_{uniqueId}",
                    fullName = "First User",
                    email,
                    role = "Employee",
                    password
                })
            .When()
                .Post($"{BaseUrl}/api/auth/register")
            .Then()
                .StatusCode(200);

            // Second registration with the same email must fail
            Given()
                .ContentType("application/json")
                .Body(new
                {
                    userName = $"user2_{uniqueId}",
                    fullName = "Second User",
                    email,
                    role = "Employee",
                    password
                })
            .When()
                .Post($"{BaseUrl}/api/auth/register")
            .Then()
                .StatusCode(NHamcrest.Is.GreaterThanOrEqualTo(400));
        }

        // =========================================================================
        // 2. Resource Not Found (404) Unhappy Paths
        // =========================================================================

        [Fact]
        public void CreateTask_WithNonExistentProjectId_Returns404NotFound()
        {
            var (managerToken, _, _) = RegisterAndLogin("Manager");
            var nonExistentProjectId = Guid.NewGuid();

            Given()
                .ContentType("application/json")
                .OAuth2(managerToken)
                .Body(new
                {
                    title = "Orphan Task",
                    description = "Task referencing nonexistent project",
                    projectId = nonExistentProjectId
                })
            .When()
                .Post($"{BaseUrl}/api/task")
            .Then()
                .StatusCode(404)
                .Body("message", NHamcrest.Contains.String($"Project with Id {nonExistentProjectId} is Not Found"));
        }

        [Fact]
        public void AssignTask_WithNonExistentTaskId_Returns404NotFound()
        {
            var (managerToken, _, _) = RegisterAndLogin("Manager");
            var (_, employeeId, _) = RegisterAndLogin("Employee");
            var nonExistentTaskId = Guid.NewGuid();

            Given()
                .ContentType("application/json")
                .OAuth2(managerToken)
                .Body(new
                {
                    assignedUserId = employeeId,
                    taskId = nonExistentTaskId
                })
            .When()
                .Post($"{BaseUrl}/api/task/assign")
            .Then()
                .StatusCode(404)
                .Body("message", NHamcrest.Contains.String($"Task with Id {nonExistentTaskId} is Not Found"));
        }

        [Fact]
        public void AssignTask_WithNonExistentUserId_Returns404NotFound()
        {
            var (managerToken, _, _) = RegisterAndLogin("Manager");
            var projectId = CreateProject(managerToken, "Assign Test Project");
            var taskId = CreateTask(managerToken, projectId, "Task for Invalid User");
            var nonExistentUserId = Guid.NewGuid().ToString();

            Given()
                .ContentType("application/json")
                .OAuth2(managerToken)
                .Body(new
                {
                    assignedUserId = nonExistentUserId,
                    taskId = Guid.Parse(taskId)
                })
            .When()
                .Post($"{BaseUrl}/api/task/assign")
            .Then()
                .StatusCode(404)
                .Body("message", NHamcrest.Contains.String($"User with Id {nonExistentUserId} is Not Found"));
        }

        [Fact]
        public void MarkTaskAsDone_WithNonExistentTaskId_Returns404NotFound()
        {
            var (employeeToken, _, _) = RegisterAndLogin("Employee");
            var nonExistentTaskId = Guid.NewGuid();

            Given()
                .Accept("application/json")
                .OAuth2(employeeToken)
            .When()
                .Put($"{BaseUrl}/api/user-tasks/mark?taskId={nonExistentTaskId}")
            .Then()
                .StatusCode(404)
                .Body("message", NHamcrest.Contains.String($"Task with Id {nonExistentTaskId} is Not Found"));
        }

        [Fact]
        public void GetProjectById_WithNonExistentProjectId_Returns404NotFound()
        {
            var (managerToken, _, _) = RegisterAndLogin("Manager");
            var nonExistentProjectId = Guid.NewGuid();

            Given()
                .Accept("application/json")
                .OAuth2(managerToken)
            .When()
                .Get($"{BaseUrl}/api/project/{nonExistentProjectId}")
            .Then()
                .StatusCode(404)
                .Body("message", NHamcrest.Contains.String($"Project with id {nonExistentProjectId} is not found"));
        }

        [Fact]
        public void GetTaskById_WithNonExistentTaskId_Returns404NotFound()
        {
            var (managerToken, _, _) = RegisterAndLogin("Manager");
            var nonExistentTaskId = Guid.NewGuid();

            Given()
                .Accept("application/json")
                .OAuth2(managerToken)
            .When()
                .Get($"{BaseUrl}/api/task/{nonExistentTaskId}")
            .Then()
                .StatusCode(404)
                .Body("message", NHamcrest.Contains.String($"Task with Id {nonExistentTaskId} is Not Found"));
        }

        [Fact]
        public void DeleteProject_WithNonExistentProjectId_Returns404NotFound()
        {
            var (managerToken, _, _) = RegisterAndLogin("Manager");
            var nonExistentProjectId = Guid.NewGuid();

            Given()
                .OAuth2(managerToken)
            .When()
                .Delete($"{BaseUrl}/api/project/{nonExistentProjectId}")
            .Then()
                .StatusCode(404)
                .Body("message", NHamcrest.Contains.String($"Project with id {nonExistentProjectId} is not found"));
        }

        [Fact]
        public void DeleteTask_WithNonExistentTaskId_Returns404NotFound()
        {
            var (managerToken, _, _) = RegisterAndLogin("Manager");
            var nonExistentTaskId = Guid.NewGuid();

            Given()
                .OAuth2(managerToken)
            .When()
                .Delete($"{BaseUrl}/api/task?taskId={nonExistentTaskId}")
            .Then()
                .StatusCode(404)
                .Body("message", NHamcrest.Contains.String($"Task with Id {nonExistentTaskId} is Not Found"));
        }
    }
}
