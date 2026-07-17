# FixFlow

[![Backend CI](https://github.com/Abdullah-esmail20/FixFlow/actions/workflows/backend-ci.yml/badge.svg)](https://github.com/Abdullah-esmail20/FixFlow/actions/workflows/backend-ci.yml)

FixFlow is a maintenance request management system built with ASP.NET Core Web API and Clean Architecture.

The system allows customers to create maintenance requests, admins to assign technicians, technicians to manage the work process, and customers to confirm completed services.

## Main Features

- Customer, technician, and admin authentication
- JWT authentication
- Role-based authorization
- Swagger documentation with JWT support
- Maintenance request creation
- Technician assignment
- Full maintenance request workflow
- Customer confirmation
- Service categories
- Pagination
- Filtering
- Search
- Sorting
- DTO validation
- Standard validation error responses
- Standard API response wrapper
- Global exception handling middleware
- Default development admin seeding
- User Secrets for sensitive configuration
- SQL Server LocalDB
- Entity Framework Core migrations
- Domain unit tests
- GitHub Actions CI

## Maintenance Request Workflow

```text
Created
→ Assigned
→ Accepted
→ InProgress
→ Completed
→ CustomerConfirmed
```

## Tech Stack

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server LocalDB
- ASP.NET Core Identity
- JWT Authentication
- Clean Architecture
- Repository Pattern
- xUnit
- Swagger / OpenAPI
- GitHub Actions

## Architecture

The backend is structured using Clean Architecture principles.

### FixFlow.Domain

Contains core business entities, enums, and business rules.

### FixFlow.Application

Contains DTOs, interfaces, services, application logic, validation models, and shared result objects.

### FixFlow.Infrastructure

Contains database context, repositories, Identity, EF Core configuration, persistence logic, and seeders.

### FixFlow.API

Contains API controllers, authentication configuration, Swagger configuration, middleware, response wrappers, and application startup.

## Project Structure

```text
FixFlow/
├── backend/
│   ├── FixFlow.API
│   ├── FixFlow.Application
│   ├── FixFlow.Domain
│   └── FixFlow.Infrastructure
├── tests/
│   └── FixFlow.Domain.Tests
├── .github/
│   └── workflows/
│       └── backend-ci.yml
├── FixFlow.slnx
└── README.md
```

## API Documentation

Swagger UI is available in development mode at:

```text
https://localhost:7153/swagger
```

You can use Swagger to explore and test the API endpoints.

For protected endpoints, click **Authorize** and paste the JWT token only.

Do not write `Bearer` manually.

## API Response Format

Successful responses use a standard response wrapper:

```json
{
  "success": true,
  "message": "Request completed successfully.",
  "data": {},
  "errors": []
}
```

Validation errors also use a standard format:

```json
{
  "success": false,
  "message": "Validation failed.",
  "data": null,
  "errors": [
    "Email: The Email field is not a valid e-mail address."
  ]
}
```

## API Endpoints

### Authentication

```http
POST /api/auth/register
POST /api/auth/login
```

### Service Categories

```http
GET /api/categories
```

### Maintenance Requests

```http
POST /api/maintenance-requests
GET /api/maintenance-requests/my-requests
GET /api/maintenance-requests/my-assigned
GET /api/maintenance-requests/{id}
PUT /api/maintenance-requests/{id}/assign-technician
PUT /api/maintenance-requests/{id}/accept
PUT /api/maintenance-requests/{id}/start-work
PUT /api/maintenance-requests/{id}/complete
PUT /api/maintenance-requests/{id}/confirm
```

### Admin Pagination, Filtering, Search, and Sorting

```http
GET /api/maintenance-requests/admin?PageNumber=1&PageSize=10
GET /api/maintenance-requests/admin?PageNumber=1&PageSize=10&Search=internet
GET /api/maintenance-requests/admin?PageNumber=1&PageSize=10&Status=Completed
GET /api/maintenance-requests/admin?PageNumber=1&PageSize=10&SortBy=CreatedAt&SortDirection=desc
```

Supported query parameters:

- `PageNumber`
- `PageSize`
- `Status`
- `Priority`
- `ServiceCategoryId`
- `Search`
- `SortBy`
- `SortDirection`

Search works on:

- `Title`
- `Description`
- `Location`

Supported sorting fields:

- `CreatedAt`
- `UpdatedAt`
- `Title`
- `Status`
- `Priority`

Supported sort directions:

- `asc`
- `desc`

## Roles

### Customer

Customers can:

- Create maintenance requests
- View their own requests
- View request details
- Confirm completed requests

### Technician

Technicians can:

- View assigned requests
- Accept assigned requests
- Start work
- Complete work
- View assigned request details

### Admin

Admins can:

- View maintenance requests
- Search, filter, sort, and paginate maintenance requests
- Assign technicians to maintenance requests

## Authentication

The project uses JWT authentication.

After login, the API returns a JWT token. Protected endpoints require this header:

```http
Authorization: Bearer YOUR_TOKEN_HERE
```

## Database

The project uses SQL Server LocalDB.

Default connection string:

```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=FixFlowDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

## Default Development Admin

In development mode, the system can seed a default admin user automatically.

The admin email is configured in `appsettings.Development.json`.

The admin password should be configured locally using User Secrets:

```bash
cd backend/FixFlow.API
dotnet user-secrets init
dotnet user-secrets set "AdminSeed:Password" "YOUR_LOCAL_PASSWORD"
```

This keeps the password outside the repository.

## How to Run

1. Clone the repository.

```bash
git clone https://github.com/Abdullah-esmail20/FixFlow.git
```

2. Open the project folder.

```bash
cd FixFlow
```

3. Restore packages.

```bash
dotnet restore FixFlow.slnx
```

4. Apply database migrations.

```bash
dotnet ef database update --project ./backend/FixFlow.Infrastructure/FixFlow.Infrastructure.csproj --startup-project ./backend/FixFlow.API/FixFlow.API.csproj
```

5. Run the API.

```bash
dotnet run --project ./backend/FixFlow.API/FixFlow.API.csproj
```

6. Open Swagger.

```text
https://localhost:7153/swagger
```

## Tests

The project includes unit tests for the domain workflow.

Run tests with:

```bash
dotnet test FixFlow.slnx
```

Tested workflow rules include:

- Created request can be assigned
- Assigned request can be accepted
- Accepted request can start work
- In-progress request can be completed
- Completed request can be confirmed by customer
- Invalid workflow transitions are rejected

## GitHub Actions

The project includes a Backend CI workflow.

On every push to `master`, GitHub Actions runs:

- Restore
- Build
- Test

Workflow file:

```text
.github/workflows/backend-ci.yml
```

## Current Status

The backend currently supports:

- Clean Architecture setup
- Database persistence
- Identity tables
- JWT authentication
- Role-based authorization
- Swagger documentation
- DTO validation
- Standard validation error responses
- Standard API response wrapper
- Global exception handling
- Full maintenance request lifecycle
- Pagination, filtering, search, and sorting
- Default development admin seeding
- User Secrets
- Domain unit tests
- GitHub Actions CI

## Future Improvements

- React frontend
- Refresh tokens
- Email verification
- Admin dashboard
- Technician dashboard
- Advanced reporting
- Deployment
- Integration tests

## Purpose

This project is built as a portfolio project to demonstrate backend development skills using ASP.NET Core, Clean Architecture, Entity Framework Core, Identity, JWT, SQL Server, unit testing, secure configuration, and CI automation.