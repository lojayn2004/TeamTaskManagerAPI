using Xunit;
using static RestAssured.Dsl;
using NHamcrest;

namespace TeamTaskManager.Tests
{
    /// <summary>
    /// Integration tests for Authorization policies (ManagerOnly, EmployeeOnly, and Unauthenticated access).
    /// </summary>
    public class AuthorizationPolicyTests : TestBase
    {
        // =========================================================================
        // 1. Unauthenticated Requests (No Bearer Token) -> 401 Unauthorized
        // =========================================================================

        [Fact]
        public void UnauthenticatedRequest_ToManagerEndpoint_Returns401Unauthorized()
        {
            // Missing Authorization header on Manager-only endpoints
            Given()
                .ContentType("application/json")
                .Body(new { name = "Unauthorized Project", description = "Should fail" })
            .When()
                .Post($"{BaseUrl}/api/project")
            .Then()
                .StatusCode(401);

            Given()
                .ContentType("application/json")
                .Body(new { title = "Unauthorized Task", description = "Should fail", projectId = Guid.NewGuid() })
            .When()
                .Post($"{BaseUrl}/api/task")
            .Then()
                .StatusCode(401);

            Given()
                .ContentType("application/json")
                .Body(new { assignedUserId = "some-user-id", taskId = Guid.NewGuid() })
            .When()
                .Post($"{BaseUrl}/api/task/assign")
            .Then()
                .StatusCode(401);
        }

        [Fact]
        public void UnauthenticatedRequest_ToEmployeeEndpoint_Returns401Unauthorized()
        {
            // Missing Authorization header on Employee-only endpoints
            Given()
                .Accept("application/json")
            .When()
                .Get($"{BaseUrl}/api/user-tasks")
            .Then()
                .StatusCode(401);

            Given()
                .Accept("application/json")
            .When()
                .Put($"{BaseUrl}/api/user-tasks/mark?taskId={Guid.NewGuid()}")
            .Then()
                .StatusCode(401);
        }

        // =========================================================================
        // 2. Employee Accessing Manager-Only Endpoints -> 403 Forbidden
        // =========================================================================

        [Fact]
        public void Employee_AccessingManagerEndpoints_Returns403Forbidden()
        {
            var (employeeToken, _, _) = RegisterAndLogin("Employee");

            // Employee trying to create a project
            Given()
                .ContentType("application/json")
                .OAuth2(employeeToken)
                .Body(new { name = "Forbidden Project", description = "Employee should not create projects" })
            .When()
                .Post($"{BaseUrl}/api/project")
            .Then()
                .StatusCode(403);

            // Employee trying to get all projects
            Given()
                .Accept("application/json")
                .OAuth2(employeeToken)
            .When()
                .Get($"{BaseUrl}/api/project")
            .Then()
                .StatusCode(403);

            // Employee trying to create a task
            Given()
                .ContentType("application/json")
                .OAuth2(employeeToken)
                .Body(new { title = "Forbidden Task", description = "Employee should not create tasks", projectId = Guid.NewGuid() })
            .When()
                .Post($"{BaseUrl}/api/task")
            .Then()
                .StatusCode(403);

            // Employee trying to assign a task
            Given()
                .ContentType("application/json")
                .OAuth2(employeeToken)
                .Body(new { assignedUserId = "target-user", taskId = Guid.NewGuid() })
            .When()
                .Post($"{BaseUrl}/api/task/assign")
            .Then()
                .StatusCode(403);

            // Employee trying to get all tasks
            Given()
                .Accept("application/json")
                .OAuth2(employeeToken)
            .When()
                .Get($"{BaseUrl}/api/task")
            .Then()
                .StatusCode(403);

            // Employee trying to delete a project
            Given()
                .OAuth2(employeeToken)
            .When()
                .Delete($"{BaseUrl}/api/project?projectId={Guid.NewGuid()}")
            .Then()
                .StatusCode(403);

            // Employee trying to delete a task
            Given()
                .OAuth2(employeeToken)
            .When()
                .Delete($"{BaseUrl}/api/task?taskId={Guid.NewGuid()}")
            .Then()
                .StatusCode(403);
        }

        // =========================================================================
        // 3. Manager Accessing Employee-Only Endpoints -> 403 Forbidden
        // =========================================================================

        [Fact]
        public void Manager_AccessingEmployeeEndpoints_Returns403Forbidden()
        {
            var (managerToken, _, _) = RegisterAndLogin("Manager");

            // Manager trying to access employee's personal user-tasks endpoint
            Given()
                .Accept("application/json")
                .OAuth2(managerToken)
            .When()
                .Get($"{BaseUrl}/api/user-tasks")
            .Then()
                .StatusCode(403);

            // Manager trying to mark a task as done through employee user-tasks endpoint
            Given()
                .Accept("application/json")
                .OAuth2(managerToken)
            .When()
                .Put($"{BaseUrl}/api/user-tasks/mark?taskId={Guid.NewGuid()}")
            .Then()
                .StatusCode(403);
        }
    }
}
