\# FixFlow



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





Project Structure

backend/

├── FixFlow.API

├── FixFlow.Application

├── FixFlow.Domain

├── FixFlow.Infrastructure

└── FixFlow.sln



Tech Stack

ASP.NET Core Web API

C#

Entity Framework Core

SQL Server LocalDB

ASP.NET Core Identity

JWT Authentication

Clean Architecture

Repository Pattern

Architecture



The backend is structured using Clean Architecture principles:



FixFlow.Domain



Contains core business entities, enums, and business rules.



FixFlow.Application



Contains DTOs, interfaces, services, and application logic.



FixFlow.Infrastructure



Contains database context, repositories, Identity, EF Core configuration, and persistence logic.



FixFlow.API



Contains API controllers, authentication configuration, and application startup.



API Endpoints

Authentication

POST /api/auth/register

POST /api/auth/login

Service Categories

GET /api/categories

Maintenance Requests

POST /api/maintenance-requests

GET /api/maintenance-requests/my-requests

GET /api/maintenance-requests/my-assigned

GET /api/maintenance-requests/{id}

PUT /api/maintenance-requests/{id}/assign-technician

PUT /api/maintenance-requests/{id}/accept

PUT /api/maintenance-requests/{id}/start-work

PUT /api/maintenance-requests/{id}/complete

PUT /api/maintenance-requests/{id}/confirm

Roles

Customer



Customers can:



Create maintenance requests

View their own requests

View request details

Confirm completed requests

Technician



Technicians can:



View assigned requests

Accept assigned requests

Start work

Complete work

View assigned request details

Admin



Admins can:



Assign technicians to maintenance requests

Authentication



The project uses JWT authentication.



After login, the API returns a JWT token.

Protected endpoints require this header:



Authorization: Bearer YOUR\_TOKEN\_HERE

Database



The project uses SQL Server LocalDB.



Default connection string:



"DefaultConnection": "Server=(localdb)\\\\MSSQLLocalDB;Database=FixFlowDb;Trusted\_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

How to Run

Clone the repository.

git clone https://github.com/Abdullah-esmail20/FixFlow.git

Open the backend folder.

cd FixFlow/backend

Restore packages.

dotnet restore

Apply database migrations.

dotnet ef database update --project ./FixFlow.Infrastructure/FixFlow.Infrastructure.csproj --startup-project ./FixFlow.API/FixFlow.API.csproj

Run the API.

dotnet run --project ./FixFlow.API/FixFlow.API.csproj

Current Status



The backend currently supports:



Clean Architecture setup

Database persistence

Identity tables

JWT authentication

Role-based authorization

Full maintenance request lifecycle.

