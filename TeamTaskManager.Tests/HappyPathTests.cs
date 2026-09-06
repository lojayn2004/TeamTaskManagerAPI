using System.Text.Json;
using Xunit;
using static RestAssured.Dsl;
using NHamcrest;

namespace TeamTaskManager.Tests
{
    public class HappyPathTests : TestBase
    {
        [Fact]
        public void CompleteLifecycle_HappyPath_FromUserCreationToTaskDone()
        {
            var uniqueId = GetUniqueId();
            var managerEmail = $"manager_{uniqueId}@taskmanager.com";
            var employeeEmail = $"employee_{uniqueId}@taskmanager.com";
            var defaultPassword = "SecurePassword123!";

            // =========================================================================
            // 1. Register a Manager
            // =========================================================================
            var registerManagerPayload = new
            {
                userName = $"manager_{uniqueId}",
                fullName = $"Manager User {uniqueId}",
                email = managerEmail,
                role = "Manager",
                password = defaultPassword
            };

            Given()
                .ContentType("application/json")
                .Body(registerManagerPayload)
            .When()
                .Post($"{BaseUrl}/api/auth/register")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true));

            // =========================================================================
            // 2. Login as Manager (Retrieve Auth Token)
            // =========================================================================
            var loginManagerPayload = new
            {
                email = managerEmail,
                password = defaultPassword
            };

            var managerLoginResponse = Given()
                .ContentType("application/json")
                .Body(loginManagerPayload)
            .When()
                .Post($"{BaseUrl}/api/auth/login")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true))
                .Body("data.token", NHamcrest.Is.Not(NHamcrest.Is.Null()))
                .Extract().Body();

            using var managerDoc = ParseJson(managerLoginResponse);
            var managerToken = managerDoc.RootElement.GetProperty("data").GetProperty("token").GetString();
            Assert.False(string.IsNullOrWhiteSpace(managerToken), "Manager token must not be null or empty");

            // =========================================================================
            // 3. Register an Employee
            // =========================================================================
            var registerEmployeePayload = new
            {
                userName = $"employee_{uniqueId}",
                fullName = $"Employee User {uniqueId}",
                email = employeeEmail,
                role = "Employee",
                password = defaultPassword
            };

            Given()
                .ContentType("application/json")
                .Body(registerEmployeePayload)
            .When()
                .Post($"{BaseUrl}/api/auth/register")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true));

            // =========================================================================
            // 4. Login as Employee (Retrieve Auth Token and User ID)
            // =========================================================================
            var loginEmployeePayload = new
            {
                email = employeeEmail,
                password = defaultPassword
            };

            var employeeLoginResponse = Given()
                .ContentType("application/json")
                .Body(loginEmployeePayload)
            .When()
                .Post($"{BaseUrl}/api/auth/login")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true))
                .Body("data.token", NHamcrest.Is.Not(NHamcrest.Is.Null()))
                .Body("data.userId", NHamcrest.Is.Not(NHamcrest.Is.Null()))
                .Extract().Body();

            using var employeeDoc = ParseJson(employeeLoginResponse);
            var employeeToken = employeeDoc.RootElement.GetProperty("data").GetProperty("token").GetString();
            var employeeUserId = employeeDoc.RootElement.GetProperty("data").GetProperty("userId").GetString();

            Assert.False(string.IsNullOrWhiteSpace(employeeToken), "Employee token must not be null or empty");
            Assert.False(string.IsNullOrWhiteSpace(employeeUserId), "Employee userId must not be null or empty");

            // =========================================================================
            // 5. Create a Project (As Manager)
            // =========================================================================
            var createProjectPayload = new
            {
                name = $"Release v1.0 - {uniqueId}",
                description = "Integration test project for happy path verification"
            };

            var createProjectResponse = Given()
                .ContentType("application/json")
                .OAuth2(managerToken!)
                .Body(createProjectPayload)
            .When()
                .Post($"{BaseUrl}/api/project")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true))
                .Body("data.name", NHamcrest.Is.EqualTo(createProjectPayload.name))
                .Extract().Body();

            using var projectDoc = ParseJson(createProjectResponse);
            var projectId = projectDoc.RootElement.GetProperty("data").GetProperty("id").GetString();
            Assert.False(string.IsNullOrWhiteSpace(projectId), "Project ID must not be null or empty");

            // =========================================================================
            // 6. Create a Task under the Project (As Manager)
            // =========================================================================
            var createTaskPayload = new
            {
                title = $"Configure CI/CD Pipeline - {uniqueId}",
                description = "Setup automated build and test pipeline for TeamTaskManager",
                projectId = Guid.Parse(projectId!)
            };

            var createTaskResponse = Given()
                .ContentType("application/json")
                .OAuth2(managerToken!)
                .Body(createTaskPayload)
            .When()
                .Post($"{BaseUrl}/api/task")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true))
                .Body("data.title", NHamcrest.Is.EqualTo(createTaskPayload.title))
                .Body("data.taskStatus", NHamcrest.Is.EqualTo("Pending"))
                .Extract().Body();

            using var taskDoc = ParseJson(createTaskResponse);
            var taskId = taskDoc.RootElement.GetProperty("data").GetProperty("id").GetString();
            Assert.False(string.IsNullOrWhiteSpace(taskId), "Task ID must not be null or empty");

            // =========================================================================
            // 7. Assign the Task to the Employee (As Manager)
            // =========================================================================
            var assignTaskPayload = new
            {
                assignedUserId = employeeUserId,
                taskId = Guid.Parse(taskId!)
            };

            Given()
                .ContentType("application/json")
                .OAuth2(managerToken!)
                .Body(assignTaskPayload)
            .When()
                .Post($"{BaseUrl}/api/task/assign")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true))
                .Body("data.id", NHamcrest.Is.EqualTo(taskId))
                .Body("data.assignedUserId", NHamcrest.Is.EqualTo(employeeUserId))
                .Body("data.taskStatus", NHamcrest.Is.EqualTo("InProgress"));

            // =========================================================================
            // 8. View Tasks assigned to the Employee (As Employee)
            // =========================================================================
            var getTasksResponse = Given()
                .Accept("application/json")
                .OAuth2(employeeToken!)
            .When()
                .Get($"{BaseUrl}/api/user-tasks")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true))
                .Extract().Body();

            using var userTasksDoc = ParseJson(getTasksResponse);
            var tasksArray = userTasksDoc.RootElement.GetProperty("data").EnumerateArray();
            var assignedTaskFound = tasksArray.Any(t =>
                t.GetProperty("id").GetString() == taskId &&
                t.GetProperty("taskStatus").GetString() == "InProgress"
            );
            Assert.True(assignedTaskFound, $"Task {taskId} must be present in employee's assigned tasks with InProgress status");

            // =========================================================================
            // 9. Mark Task as Done (As Employee)
            // =========================================================================
            Given()
                .Accept("application/json")
                .OAuth2(employeeToken!)
            .When()
                .Put($"{BaseUrl}/api/user-tasks/mark?taskId={taskId}")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true))
                .Body("data.id", NHamcrest.Is.EqualTo(taskId))
                .Body("data.taskStatus", NHamcrest.Is.EqualTo("Done"));

            // =========================================================================
            // 10. Verify Final Status in Employee's Task List
            // =========================================================================
            var finalTasksResponse = Given()
                .Accept("application/json")
                .OAuth2(employeeToken!)
            .When()
                .Get($"{BaseUrl}/api/user-tasks")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true))
                .Extract().Body();

            using var finalDoc = ParseJson(finalTasksResponse);
            var completedTaskFound = finalDoc.RootElement.GetProperty("data").EnumerateArray().Any(t =>
                t.GetProperty("id").GetString() == taskId &&
                t.GetProperty("taskStatus").GetString() == "Done"
            );
            
            Assert.True(completedTaskFound, $"Task {taskId} must be marked as Done in employee's tasks");
        }
    }
}
