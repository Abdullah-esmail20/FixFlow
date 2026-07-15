\# FixFlow

[![Backend CI](https://github.com/Abdullah-esmail20/FixFlow/actions/workflows/backend-ci.yml/badge.svg)](https://github.com/Abdullah-esmail20/FixFlow/actions/workflows/backend-ci.yml)

FixFlow is a maintenance request management system built with ASP.NET Core Web API and Clean Architecture.



The system allows customers to create maintenance requests, admins to assign technicians, technicians to manage the work process, and customers to confirm completed services.



\## Main Features



\- Customer registration and login

\- Technician registration and login

\- Admin registration and login

\- JWT authentication

\- Role-based authorization

\- Maintenance request creation

\- Assign technician to request

\- Technician workflow:

&#x20; - Accept request

&#x20; - Start work

&#x20; - Complete work

\- Customer confirmation

\- Service categories

\- SQL Server database

\- Entity Framework Core migrations



\## Maintenance Request Workflow



```text

Created

→ Assigned

→ Accepted

→ InProgress

→ Completed

→ CustomerConfirmed

```

\## API Documentation



Swagger UI is available in development mode at:



```text

https://localhost:7153/swagger



بعدها ننتقل لتحسين احترافي مهم: \*\*إضافة validation للـDTOs\*\* عشان لو المستخدم أرسل بيانات ناقصة يرجع أخطاء واضحة.



\## Tech Stack



\- ASP.NET Core Web API

\- C#

\- Entity Framework Core

\- SQL Server LocalDB

\- ASP.NET Core Identity

\- JWT Authentication

\- Clean Architecture

\- Repository Pattern



\## Architecture



The backend is structured using Clean Architecture principles:



\### FixFlow.Domain



Contains core business entities, enums, and business rules.



\### FixFlow.Application



Contains DTOs, interfaces, services, and application logic.



\### FixFlow.Infrastructure



Contains database context, repositories, Identity, EF Core configuration, and persistence logic.



\### FixFlow.API



Contains API controllers, authentication configuration, and application startup.



\## Project Structure



```text

backend/

├── FixFlow.API

├── FixFlow.Application

├── FixFlow.Domain

├── FixFlow.Infrastructure

└── FixFlow.sln

```



\## API Endpoints



\### Authentication



```http

POST /api/auth/register

POST /api/auth/login

```



\### Service Categories



```http

GET /api/categories

```



\### Maintenance Requests



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



\## Roles



\### Customer



Customers can:



\- Create maintenance requests

\- View their own requests

\- View request details

\- Confirm completed requests



\### Technician



Technicians can:



\- View assigned requests

\- Accept assigned requests

\- Start work

\- Complete work

\- View assigned request details



\### Admin



Admins can:



\- Assign technicians to maintenance requests



\## Authentication



The project uses JWT authentication.



After login, the API returns a JWT token. Protected endpoints require this header:



```http

Authorization: Bearer YOUR\\\_TOKEN\\\_HERE

```



\## Database



The project uses SQL Server LocalDB.



Default connection string:



```json

"DefaultConnection": "Server=(localdb)\\\\\\\\MSSQLLocalDB;Database=FixFlowDb;Trusted\\\_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

```



\## How to Run



1\. Clone the repository.



```bash

git clone https://github.com/Abdullah-esmail20/FixFlow.git

```



2\. Open the backend folder.



```bash

cd FixFlow/backend

```



3\. Restore packages.



```bash

dotnet restore

```



4\. Apply database migrations.



```bash

dotnet ef database update --project ./FixFlow.Infrastructure/FixFlow.Infrastructure.csproj --startup-project ./FixFlow.API/FixFlow.API.csproj

```



5\. Run the API.



```bash

dotnet run --project ./FixFlow.API/FixFlow.API.csproj

```



\## Current Status



The backend currently supports:



\- Clean Architecture setup

\- Database persistence

\- Identity tables

\- JWT authentication

\- Role-based authorization

\- Full maintenance request lifecycle



\## Future Improvements



\- React frontend

\- Swagger UI

\- Refresh tokens

\- Email verification

\- Admin dashboard

\- Technician dashboard

\- Request filtering and pagination

\- Unit tests
## Tests

The project includes unit tests for the domain workflow.

```bash
dotnet test .\FixFlow.slnx


\## Purpose

<img width="1535" height="567" alt="image" src="https://github.com/user-attachments/assets/52947046-5cd2-4256-8f0a-ba041a2c03b2" />




This project is built as a portfolio project to demonstrate backend development skills using ASP.NET Core, Clean Architecture, Entity Framework Core, Identity, JWT, and SQL Server.
## Pagination, Filtering and Search

Admins can retrieve maintenance requests using pagination, filtering, and search.

Example:

```http
GET /api/maintenance-requests/admin?PageNumber=1&PageSize=10&Search=internet
