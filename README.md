# TeamTaskManager API

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-13.0-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-10.0-512BD4?logo=nuget&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![SignalR](https://img.shields.io/badge/SignalR-Real--Time-orange?logo=signalr&logoColor=white)](https://learn.microsoft.com/aspnet/core/signalr/)
[![xUnit](https://img.shields.io/badge/Tests-xUnit%20%2B%20RestAssured.Net-blue?logo=xunit&logoColor=white)](https://xunit.net/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

A Task and Team Management RESTful API built with **ASP.NET Core 10**. Features Role-Based Access Control (RBAC) powered by ASP.NET Core Identity and JWT, the Specification and Repository patterns for clean data access, real-time push notifications via SignalR, and a comprehensive end-to-end integration testing suite.

---

## Table of Contents

- [Key Features](#key-features)
- [Tech Stack](#tech-stack)
- [Role-Based Access Control (RBAC)](#role-based-access-control-rbac)
- [API Endpoints Reference](#api-endpoints-reference)
- [Integration Testing Suite](#integration-testing-suite)
  - [Honesty & Transparency Disclosure](#honesty--transparency-disclosure)
  - [Running Testcases](#running-testcases)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Database Configuration](#database-configuration)
  - [Database Migrations & Seeding](#database-migrations--seeding)
  - [Running the Application](#running-the-application)

--

## Key Features

- **JWT Authentication & Authorization**: Secure token-based authentication with ASP.NET Core Identity supporting distinct role policies (`ManagerOnly` and `EmployeeOnly`).
- **Role-Based Workflow**:
  - **Managers** create projects, create tasks, assign tasks to employees, and monitor progress.
  - **Employees** view their personal workload and mark assigned tasks as completed.
- **Strict Data Isolation**: Multi-tenant-like data scoping preventing employees from viewing or completing tasks assigned to other colleagues.
- **Real-Time SignalR Hub**: Automatic push notifications sent directly to the project creator whenever an assigned employee marks a task as `Done`.
- **Notification Persistence**: In addition to real-time delivery, notifications are stored in SQL Server for persistent audit trails.
- **Design Patterns**:
  - **Repository Pattern**: Decouples business logic from EF Core data access.
  - **Specification Pattern**: Reusable, composable query criteria with strong typing and eager loading (`QueryEvaluator`).
  - **Result Pattern**: Service operations return `ServiceResult<T>` with standardized error types (`NotFound`, `UnAuthorized`, `Validation`), mapped consistently to HTTP status codes by `ApiBaseController`. 

---




## Tech Stack

| Component | Technology | Description |
| :--- | :--- | :--- |
| **Runtime / Framework** | .NET 10 (C# 13) | High-performance modern web framework |
| **Data Access** | Entity Framework Core 10 | Object-Relational Mapper (ORM) |
| **Database** | Microsoft SQL Server | Relational persistence |
| **Identity & Security** | ASP.NET Core Identity + JWT Bearer | Token-based auth & role claims |
| **Real-Time** | ASP.NET Core SignalR | WebSocket-based real-time push events |
| **Object Mapping** | AutoMapper 16 | Convention-based entity-to-DTO mapping |
| **API Documentation** | Swashbuckle / OpenAPI (Swagger) | Interactive API exploration |
| **Testing** | xUnit, RestAssured.Net, NHamcrest | Automated integration test suite |

---

## Role-Based Access Control (RBAC)

The system enforces two primary roles with explicit policy boundaries:

| Resource / Action | Endpoint | Manager | Employee | Unauthenticated |
| :--- | :--- | :---: | :---: | :---: |
| Register User | `POST /api/auth/register` | ✅ | ✅ | ✅ |
| User Login | `POST /api/auth/login` | ✅ | ✅ | ✅ |
| Create Project | `POST /api/project` | ✅ | ❌ (403) | ❌ (401) |
| List All Projects | `GET /api/project` | ✅ | ❌ (403) | ❌ (401) |
| Get Project by ID | `GET /api/project/projectId` | ✅ | ❌ (403) | ❌ (401) |
| Update Project | `PUT /api/project` | ✅ | ❌ (403) | ❌ (401) |
| Delete Project | `DELETE /api/project` | ✅ | ❌ (403) | ❌ (401) |
| Create Task | `POST /api/task` | ✅ | ❌ (403) | ❌ (401) |
| List All Tasks | `GET /api/task` | ✅ | ❌ (403) | ❌ (401) |
| Get Task by ID | `GET /api/task/taskId` | ✅ | ❌ (403) | ❌ (401) |
| Update Task | `PUT /api/task` | ✅ | ❌ (403) | ❌ (401) |
| Delete Task | `DELETE /api/task` | ✅ | ❌ (403) | ❌ (401) |
| Assign Task | `POST /api/task/assign` | ✅ | ❌ (403) | ❌ (401) |
| View My Tasks | `GET /api/user-tasks` | ❌ (403) | ✅ | ❌ (401) |
| Complete Task | `PUT /api/user-tasks/mark` | ❌ (403) | ✅ *(Own only)* | ❌ (401) |

---

## API Endpoints Reference

### 1. Authentication (`/api/auth`)

#### `POST /api/auth/register`
Registers a new user account.
```json
// Request Body
{
  "userName": "johndoe",
  "fullName": "John Doe",
  "email": "johndoe@taskmanager.com",
  "role": "Manager", // "Manager" or "Employee"
  "password": "SecurePassword123!"
}

// Response (200 OK)
{
  "success": true,
  "data": {
    "userId": "d7a4b890-...",
    "email": "johndoe@taskmanager.com",
    "token": "eyJhbGciOiJIUzI1NiIs..."
  }
}
```

#### `POST /api/auth/login`
Authenticates a user and issues a JWT token.
```json
// Request Body
{
  "email": "johndoe@taskmanager.com",
  "password": "SecurePassword123!"
}

// Response (200 OK)
{
  "success": true,
  "data": {
    "userId": "d7a4b890-...",
    "email": "johndoe@taskmanager.com",
    "token": "eyJhbGciOiJIUzI1NiIs..."
  }
}
```

---

### 2. Project Management (`/api/project`)
> **Policy**: `ManagerOnly` (Requires Bearer token with `Manager` role)

- **`POST /api/project`**: Creates a project.
  ```json
  {
    "name": "Q3 Infrastructure Overhaul",
    "description": "Migrate backend services and setup CI/CD"
  }
  ```
- **`GET /api/project`**: Returns all projects.
- **`GET /api/project/projectId?projectId={guid}`**: Returns a project by its unique ID.
- **`PUT /api/project`**: Updates project title and description.
- **`DELETE /api/project?projectId={guid}`**: Removes a project.

---

### 3. Task Management (`/api/task`)
> **Policy**: `ManagerOnly` (Requires Bearer token with `Manager` role)

- **`POST /api/task`**: Creates a new task in `Pending` status.
  ```json
  {
    "title": "Configure GitHub Actions",
    "description": "Set up build, test, and release workflows",
    "projectId": "7c9e6679-7425-40de-944b-e07fc1f90ae7"
  }
  ```
- **`GET /api/task`**: Returns all tasks across all projects.
- **`GET /api/task/taskId?taskId={guid}`**: Returns a specific task.
- **`PUT /api/task`**: Updates task details, status, or assignment.
- **`DELETE /api/task?taskId={guid}`**: Deletes a task.
- **`POST /api/task/assign`**: Assigns a task to an employee and automatically sets its status to `InProgress`.
  ```json
  {
    "taskId": "9a3841a1-9ef0-4c7b-944f-c4f52622be48",
    "assignedUserId": "b1b705ef-2f47-4f40-8774-7221e64cf123"
  }
  ```

---

### 4. Employee Personal Workload (`/api/user-tasks`)
> **Policy**: `EmployeeOnly` (Requires Bearer token with `Employee` role)

- **`GET /api/user-tasks`**: Returns all tasks assigned to the authenticated employee (filtered using caller's JWT claims).
- **`PUT /api/user-tasks/mark?taskId={guid}`**: Marks the specified task as `Done`.
  - Enforces ownership: returns `401 Unauthorized` if another employee or manager attempts to mark someone else's task.
  - Automatically emits a real-time SignalR notification to the project creator.
  - Persists the completion notification to the database.

---



## Integration Testing Suite

### Honesty & Transparency Disclosure

> [!NOTE]
> **Statement on AI-Assisted Testing**
>
> In the spirit of complete transparency and technical integrity, **the automated tests in this project were generated with the assistance of AI tools**.

> -  Test cases were **manually reviewed, validated, and verified** 
> - The test suite has been confirmed passing against active API instances.

### Running Testcases 

Ensure the API is running locally (e.g. at `http://localhost:5151`), then execute:

```bash
# Run all tests
dotnet test
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Microsoft SQL Server](https://www.microsoft.com/sql-server) or SQL Server LocalDB
- [Entity Framework Core Tools CLI](https://learn.microsoft.com/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)

### Database Configuration

Check or update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TaskManagement;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JWT": {
    "Secret": "",
    "Issuer": "",
    "Audience": ""
  }
}
```

### Database Migrations & Seeding

Apply the initial database schema migrations:

```bash
dotnet ef database update
```

> **Note**: Database roles (`Manager`, `Employee`) are automatically checked and seeded upon application startup via `RolesSeeding.SeedRoles`.

### Running the Application

```bash
# Launch the API
dotnet run --project TeamTaskManager.csproj
```

Once running:
- **Swagger UI**: Visit `https://localhost:7044/swagger/index.html` to explore endpoints and test requests interactively.
---


