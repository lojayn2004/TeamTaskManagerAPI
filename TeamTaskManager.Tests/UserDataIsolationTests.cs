using System.Text.Json;
using Xunit;
using static RestAssured.Dsl;
using NHamcrest;

namespace TeamTaskManager.Tests
{
    /// <summary>
    /// Integration tests verifying user data isolation and authorization scoping:
    /// - An employee only receives tasks specifically assigned to them when calling GET /api/user-tasks.
    /// - Tasks assigned to other employees or unassigned tasks are excluded.
    /// - An employee cannot mark another employee's task or an unassigned task as done.
    /// </summary>
    public class UserDataIsolationTests : TestBase
    {
        [Fact]
        public void GetUserTasks_ReturnsOnlyTasksAssignedToCallingEmployee()
        {
            // 1. Setup Manager and two Employees
            var (managerToken, _, _) = RegisterAndLogin("Manager");
            var (employee1Token, employee1Id, _) = RegisterAndLogin("Employee");
            var (employee2Token, employee2Id, _) = RegisterAndLogin("Employee");

            // 2. Manager creates a Project
            var projectId = CreateProject(managerToken, "Data Isolation Project");

            // 3. Manager creates 3 Tasks
            var task1Id = CreateTask(managerToken, projectId, "Task for Employee 1");
            var task2Id = CreateTask(managerToken, projectId, "Task for Employee 2");
            var unassignedTaskId = CreateTask(managerToken, projectId, "Unassigned Task");

            // 4. Assign Task 1 to Employee 1, and Task 2 to Employee 2
            AssignTask(managerToken, task1Id, employee1Id);
            AssignTask(managerToken, task2Id, employee2Id);

            // =========================================================================
            // Verify Employee 1 only sees Task 1
            // =========================================================================
            var emp1Response = Given()
                .Accept("application/json")
                .OAuth2(employee1Token)
            .When()
                .Get($"{BaseUrl}/api/user-tasks")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true))
                .Extract().Body();

            using var doc1 = ParseJson(emp1Response);
            var emp1Tasks = doc1.RootElement.GetProperty("data").EnumerateArray().ToList();

            // Employee 1's list MUST contain Task 1
            Assert.Contains(emp1Tasks, t => t.GetProperty("id").GetString() == task1Id);

            // Employee 1's list MUST NOT contain Task 2 (assigned to Employee 2)
            Assert.DoesNotContain(emp1Tasks, t => t.GetProperty("id").GetString() == task2Id);

            // Employee 1's list MUST NOT contain the unassigned task
            Assert.DoesNotContain(emp1Tasks, t => t.GetProperty("id").GetString() == unassignedTaskId);

            // Every task in Employee 1's response must have assignedUserId == employee1Id
            Assert.All(emp1Tasks, t =>
            {
                Assert.Equal(employee1Id, t.GetProperty("assignedUserId").GetString());
            });

            // =========================================================================
            // Verify Employee 2 only sees Task 2
            // =========================================================================
            var emp2Response = Given()
                .Accept("application/json")
                .OAuth2(employee2Token)
            .When()
                .Get($"{BaseUrl}/api/user-tasks")
            .Then()
                .StatusCode(200)
                .Body("success", NHamcrest.Is.EqualTo(true))
                .Extract().Body();

            using var doc2 = ParseJson(emp2Response);
            var emp2Tasks = doc2.RootElement.GetProperty("data").EnumerateArray().ToList();

            // Employee 2's list MUST contain Task 2
            Assert.Contains(emp2Tasks, t => t.GetProperty("id").GetString() == task2Id);

            // Employee 2's list MUST NOT contain Task 1 (assigned to Employee 1)
            Assert.DoesNotContain(emp2Tasks, t => t.GetProperty("id").GetString() == task1Id);

            // Employee 2's list MUST NOT contain the unassigned task
            Assert.DoesNotContain(emp2Tasks, t => t.GetProperty("id").GetString() == unassignedTaskId);

            // Every task in Employee 2's response must have assignedUserId == employee2Id
            Assert.All(emp2Tasks, t =>
            {
                Assert.Equal(employee2Id, t.GetProperty("assignedUserId").GetString());
            });
        }

        [Fact]
        public void MarkTaskAsDone_ByDifferentEmployee_Returns401Unauthorized()
        {
            // 1. Setup Manager and two Employees
            var (managerToken, _, _) = RegisterAndLogin("Manager");
            var (employee1Token, employee1Id, _) = RegisterAndLogin("Employee");
            var (employee2Token, _, _) = RegisterAndLogin("Employee");

            // 2. Manager creates a Project and Task, assigned to Employee 1
            var projectId = CreateProject(managerToken, "Tamper Protection Project");
            var task1Id = CreateTask(managerToken, projectId, "Employee 1 Exclusive Task");
            AssignTask(managerToken, task1Id, employee1Id);

            // 3. Employee 2 attempts to mark Employee 1's task as Done -> Should be 401 Unauthorized
            Given()
                .Accept("application/json")
                .OAuth2(employee2Token)
            .When()
                .Put($"{BaseUrl}/api/user-tasks/mark?taskId={task1Id}")
            .Then()
                .StatusCode(401)
                .Body("message", NHamcrest.Contains.String("Cannot Mark Task As Done, UnAuthorized User"));

            // 4. Verify Task 1 status was NOT changed to Done (remains InProgress)
            var getTasksResponse = Given()
                .Accept("application/json")
                .OAuth2(employee1Token)
            .When()
                .Get($"{BaseUrl}/api/user-tasks")
            .Then()
                .StatusCode(200)
                .Extract().Body();

            using var doc = ParseJson(getTasksResponse);
            var task = doc.RootElement.GetProperty("data").EnumerateArray()
                .First(t => t.GetProperty("id").GetString() == task1Id);

            Assert.Equal("InProgress", task.GetProperty("taskStatus").GetString());
        }

        [Fact]
        public void MarkTaskAsDone_OnUnassignedTask_Returns401Unauthorized()
        {
            // 1. Setup Manager and an Employee
            var (managerToken, _, _) = RegisterAndLogin("Manager");
            var (employeeToken, _, _) = RegisterAndLogin("Employee");

            // 2. Manager creates a Project and an unassigned Task
            var projectId = CreateProject(managerToken, "Unassigned Task Project");
            var unassignedTaskId = CreateTask(managerToken, projectId, "Unassigned Task");

            // 3. Employee attempts to mark unassigned task as Done -> Should be 401 Unauthorized
            Given()
                .Accept("application/json")
                .OAuth2(employeeToken)
            .When()
                .Put($"{BaseUrl}/api/user-tasks/mark?taskId={unassignedTaskId}")
            .Then()
                .StatusCode(401)
                .Body("message", NHamcrest.Contains.String("Cannot Mark Task As Done, UnAuthorized User"));
        }
    }
}
