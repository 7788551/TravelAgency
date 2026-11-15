# TravelAgency – Travel Agency Management System

[![Architecture Diagram](https://gitdiagram.com/diagram-badge.svg)](https://gitdiagram.com/7788551/travelagency?utm_source=readme&utm_medium=badge)

**TravelAgency** is an ASP.NET Core MVC and Web API application designed to manage travel destinations, travel packages, customer enquiries, user accounts, and staff operations.

The project follows a **layered architecture** using Repository Pattern, Service Layer, Dependency Injection, ADO.NET, and SQL Server. The frontend uses Razor Views, Bootstrap, jQuery, and AJAX to communicate with backend REST APIs.

The goal is to build a modular, maintainable business application while applying practical enterprise software development concepts.

---

## Architecture Overview

[![TravelAgency Architecture](https://gitdiagram.com/7788551/travelagency/diagram.png)](https://gitdiagram.com/7788551/travelagency?utm_source=readme&utm_medium=picture)

The application follows a layered architecture:

```text
Users
  |
  v
MVC Views / jQuery AJAX
  |
  v
API Controllers
  |
  v
Service Layer
  |
  v
Repository Layer
  |
  v
SQL Server
```

### Application Modules

| Module | Description |
|---|---|
| Account Management | Customer registration, login, and user account operations |
| Staff Management | Admin manages staff accounts |
| Destination Management | Create, view, update, and delete travel destinations |
| Package Management | Manage travel packages and their destinations |
| Customer Enquiries | Customers submit enquiries about travel packages |
| Enquiry Management | Staff manages customer enquiry status |
| Dashboard | Provides an entry point for role-based application navigation |

### User Roles

| Role | Responsibilities |
|---|---|
| Visitor | Browse public pages, register, and log in |
| Customer | Browse destinations and packages and submit enquiries |
| Staff | Manage destinations, packages, and customer enquiries |
| Admin | Manage staff accounts and administrative operations |

---

## Technology Stack

### Backend

| Technology | Purpose |
|---|---|
| C# | Main programming language |
| ASP.NET Core 8 | Web application framework |
| ASP.NET Core MVC | MVC controllers and Razor Views |
| ASP.NET Core Web API | REST API development |
| ADO.NET | Database access |
| Microsoft.Data.SqlClient | SQL Server connectivity |
| SQL Server | Relational database |
| BCrypt.Net-Next | Password hashing |
| ASP.NET Core Session | Session-based user state |

### Frontend

| Technology | Purpose |
|---|---|
| HTML5 | Page structure |
| CSS3 | Styling |
| Bootstrap 5 | Responsive UI |
| JavaScript | Client-side programming |
| jQuery | DOM manipulation |
| jQuery AJAX | Asynchronous API communication |

### Development Tools

- Visual Studio 2022
- SQL Server Management Studio (SSMS)
- Postman – API testing
- Git and GitHub – Version control
- GitDiagram – Architecture visualization

---

## Architecture and Design Patterns

The project uses practical software architecture patterns to separate responsibilities and make the application easier to maintain.

### 1. Repository Pattern

**Purpose:** Separates database access logic from business logic.

Repositories are responsible for communicating with SQL Server using ADO.NET.

Examples:

- `UserRepository.cs`
- `DestinationRepository.cs`
- `PackageRepository.cs`
- `CustomerRepository.cs`

### 2. Service Layer Pattern

**Purpose:** Keeps business rules and application logic separate from controllers and database operations.

Examples:

- `AccountService.cs`
- `DestinationService.cs`
- `PackageService.cs`
- `CustomerService.cs`

Services perform tasks such as validation, checking whether related records exist, password hashing, and managing business rules.

### 3. Dependency Injection

ASP.NET Core Dependency Injection is used to inject repository interfaces into services and service interfaces into API controllers.

Example:

```csharp
public class PackageService : IPackageService
{
    private readonly IPackageRepository _repository;

    public PackageService(IPackageRepository repository)
    {
        _repository = repository;
    }
}
```

Dependencies are registered through the application's service collection extension.

### 4. DTO Pattern

Data Transfer Objects are used to define the data accepted by API endpoints and returned to clients.

Examples:

- `LoginRequest`
- `RegisterRequest`
- `DestinationRequest`
- `PackageRequest`
- `EnquiryRequest`
- `UserResponse`

DTOs help prevent direct exposure of sensitive entity properties such as password hashes.

---

## OOP Concepts

The project applies object-oriented programming principles in its models, interfaces, services, and repositories.

| OOP Concept | Application |
|---|---|
| Encapsulation | Classes encapsulate their data and implementation details |
| Abstraction | Interfaces define repository and service contracts |
| Inheritance | Used where framework base classes are extended, such as MVC controllers |
| Polymorphism | Interface-based implementations allow services to depend on abstractions |

## SOLID Principles

The architecture is designed around relevant SOLID principles:

| Principle | Application |
|---|---|
| Single Responsibility Principle (SRP) | Controllers handle HTTP requests, services handle business rules, and repositories handle database operations |
| Open/Closed Principle (OCP) | Interface-based abstractions allow implementations to be extended or replaced |
| Liskov Substitution Principle (LSP) | Repository and service implementations should be usable through their interfaces |
| Interface Segregation Principle (ISP) | Separate interfaces define focused repository and service contracts |
| Dependency Inversion Principle (DIP) | Higher-level services depend on interfaces rather than concrete repositories |

---

## Security Implementation

Security is an important part of the application.

| Security Concept | Implementation |
|---|---|
| Password Hashing | BCrypt.Net-Next |
| Password Verification | BCrypt password verification |
| Session Management | ASP.NET Core Session |
| Role-Based Access Checks | Custom session authorization attribute |
| SQL Injection Prevention | Parameterized SQL queries |
| Input Validation | Request DTO validation and service-level checks |
| Sensitive Data Protection | Password hashes are not returned in user response DTOs |
| HTTPS | HTTPS redirection and secure session cookie configuration |

**Note:** The current authentication implementation uses session-based authorization. A future security improvement is to migrate to ASP.NET Core Cookie Authentication and native role-based authorization policies.

---

## Database Design

The application uses Microsoft SQL Server.

### Main Database Tables

| Table | Responsibility |
|---|---|
| Roles | Stores application roles |
| Users | Stores user accounts and password hashes |
| Destinations | Stores travel destination information |
| Packages | Stores travel packages and destination relationships |
| CustomerEnquiries | Stores customer enquiries about packages |

### Database Relationships

- One role can be assigned to multiple users.
- One destination can have multiple packages.
- One customer can submit multiple enquiries.
- One package can receive multiple enquiries.

Database constraints, primary keys, foreign keys, unique constraints, and check constraints help maintain data integrity.

---

## Frontend and API Communication

The application uses jQuery AJAX to communicate with REST API endpoints.

The request flow is:

```text
User Interaction
      |
      v
Razor View
      |
      v
jQuery AJAX
      |
      v
API Controller
      |
      v
Service
      |
      v
Repository
      |
      v
SQL Server
      |
      v
JSON Response
      |
      v
Update UI
```

MVC controllers are responsible for returning Razor Views. API controllers handle data operations and return JSON responses.

This separation keeps the frontend independent of the database implementation.

---

## Project Structure

```text
TravelAgency/
│
├── ApiController/
│   ├── AccountApiController.cs
│   ├── CustomerApiController.cs
│   ├── DestinationApiController.cs
│   ├── PackageApiController.cs
│   └── StaffApiController.cs
│
├── Controllers/
│   ├── AccountController.cs
│   ├── CustomerController.cs
│   ├── DashboardController.cs
│   ├── DestinationController.cs
│   ├── HomeController.cs
│   ├── PackageController.cs
│   └── StaffController.cs
│
├── DTOs/
│   ├── Requests/
│   └── Responses/
│
├── Interfaces/
│   ├── Repositories/
│   └── Services/
│
├── Models/
│
├── Repositories/
│   ├── UserRepository.cs
│   ├── DestinationRepository.cs
│   ├── PackageRepository.cs
│   └── CustomerRepository.cs
│
├── Services/
│   ├── AccountService.cs
│   ├── DestinationService.cs
│   ├── PackageService.cs
│   └── CustomerService.cs
│
├── Security/
│   ├── PasswordHelper.cs
│   └── SessionAuthorizationAttribute.cs
│
├── Extensions/
│   └── ServiceCollectionExtensions.cs
│
├── Database/
│   ├── Scripts/
│   └── StoredProcedures/
│
├── Views/
│   ├── Account/
│   ├── Customer/
│   ├── Dashboard/
│   ├── Destination/
│   ├── Home/
│   ├── Package/
│   ├── Staff/
│   └── Shared/
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   ├── images/
│   └── lib/
│
├── appsettings.json
├── Program.cs
└── TravelAgency.csproj
```

---

## Getting Started

### Prerequisites

Install the following:

- .NET 8 SDK
- Visual Studio 2022
- SQL Server
- SQL Server Management Studio
- Postman
- Git

### 1. Clone the Repository

```bash
git clone https://github.com/7788551/TravelAgency.git
```

### 2. Navigate to the Project

```bash
cd TravelAgency/TravelAgency
```

Adjust the directory if you cloned the repository into a different folder structure.

### 3. Configure the Database Connection

Update `appsettings.json` with your SQL Server connection string.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=TravelAgencyDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Use your actual SQL Server instance name. Do not commit production credentials or secrets to GitHub.

### 4. Install NuGet Packages

```bash
dotnet restore
```

The main application packages include:

```bash
dotnet add package Microsoft.Data.SqlClient
dotnet add package BCrypt.Net-Next
```

### 5. Create the Database

Create `TravelAgencyDB` in SQL Server and execute the SQL scripts available in the `Database/Scripts` directory.

### 6. Build the Application

```bash
dotnet build
```

### 7. Run the Application

```bash
dotnet run
```

Open the HTTPS URL displayed in the terminal.

---

## API Testing

Postman is used to test the REST API endpoints.

| Operation | HTTP Method | Endpoint |
|---|---|---|
| Customer Registration | POST | `/api/account/register` |
| Login | POST | `/api/account/login` |
| Logout | POST | `/api/account/logout` |
| User Profile | GET | `/api/account/profile` |
| Get Destinations | GET | `/api/destinations` |
| Create Destination | POST | `/api/destinations` |
| Update Destination | PUT | `/api/destinations/{id}` |
| Delete Destination | DELETE | `/api/destinations/{id}` |
| Get Packages | GET | `/api/packages` |
| Create Package | POST | `/api/packages` |
| Get My Enquiries | GET | `/api/customer/my-enquiries` |
| Get Staff | GET | `/api/staff` |

Some endpoints require an authenticated session and the appropriate role. Use the same HTTPS host and port and retain the session cookie in Postman after login.

---

## Future Enhancements

The following improvements are planned to make the application more complete and production-ready.

### Application Features

- [ ] Complete Destination CRUD frontend using jQuery AJAX.
- [ ] Complete Package CRUD frontend.
- [ ] Complete Customer Enquiry submission and management.
- [ ] Complete Staff Management interface.
- [ ] Implement a role-specific dashboard.
- [ ] Add destination and package search, filtering, and pagination.
- [ ] Add image uploads for destinations and packages.
- [ ] Add booking and reservation management.
- [ ] Add payment gateway integration.
- [ ] Add email notifications for enquiries and bookings.

### Backend and Architecture

- [ ] Add centralized exception handling middleware.
- [ ] Add structured application logging.
- [ ] Introduce consistent API response models.
- [ ] Add pagination and filtering at the database level.
- [ ] Introduce stored procedures where beneficial.
- [ ] Add database transaction management for multi-step operations.
- [ ] Improve asynchronous database operations.
- [ ] Add caching for frequently accessed data.

### Security

- [ ] Migrate to ASP.NET Core Cookie Authentication.
- [ ] Implement native role-based authorization policies.
- [ ] Add CSRF protection for state-changing browser requests.
- [ ] Add rate limiting for sensitive API endpoints.
- [ ] Move production secrets into secure configuration.
- [ ] Add security and authorization integration tests.

### Testing and Deployment

- [ ] Add unit testing with xUnit.
- [ ] Add integration testing for API endpoints.
- [ ] Add automated build and deployment using GitHub Actions.
- [ ] Containerize the application using Docker.
- [ ] Deploy the application to a cloud hosting environment.
- [ ] Configure production monitoring and health checks.

---

## Learning Objectives

This project is also a practical learning exercise in enterprise .NET development.

It provides experience with:

- Building ASP.NET Core MVC applications.
- Designing and consuming RESTful Web APIs.
- Applying OOP and SOLID principles.
- Implementing Repository and Service Layer patterns.
- Using Dependency Injection.
- Writing asynchronous ADO.NET database operations.
- Designing relational databases in SQL Server.
- Implementing authentication and authorization.
- Integrating frontend applications with APIs using AJAX.
- Organizing a maintainable modular application.

---

## Project Status

**Development in Progress**

The core application structure, database design, repository and service layers, API architecture, session-based account flow, and initial MVC frontend are being developed. Remaining features and production hardening are tracked in the Future Enhancements section.

## Repository

[GitHub – TravelAgency](https://github.com/7788551/TravelAgency)

[View Architecture on GitDiagram](https://gitdiagram.com/7788551/travelagency)

---

**Built with C#, ASP.NET Core, ADO.NET, SQL Server, Bootstrap, and jQuery.**
