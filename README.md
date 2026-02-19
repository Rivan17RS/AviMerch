# 🦅 AviMerch API

Modern .NET 8 Web API built using Clean Architecture principles.

This project is a refactored and modernized version of a previous legacy implementation, redesigned to follow scalable backend best practices using:

- .NET 8
- Entity Framework Core
- PostgreSQL
- Clean Architecture
- Swagger / OpenAPI
- Dependency Injection

---

## 🏗 Architecture

The solution follows Clean Architecture separation of concerns:

AviMerch.Domain → Core business entities  
AviMerch.Application → Application logic & use cases  
AviMerch.Infrastructure → Database & external services  
AviMerch.API → Web API entry point  

This structure ensures:
- Clear boundaries between layers
- Testability
- Scalability
- Maintainability

---

## 🚀 Tech Stack

- ASP.NET Core 8
- Entity Framework Core
- Npgsql (PostgreSQL provider)
- Swagger UI
- Dependency Injection
- RESTful API design

---

## 🗄 Database

The API connects to PostgreSQL using EF Core.

Connection string is configured in: `appsettings.json`

Make sure PostgreSQL is running before starting the API.

---

## ▶️ Running the Project

From the solution root:

```bash
dotnet build
dotnet run --project AviMerch.API
```
API will start on: `http://localhost:xxxx`
Swagger UI available at: `http://localhost:xxxx/swagger`

## 📌 Features (Current)
* Product entity management
* RESTful endpoints
* Database integration via EF Core
* Swagger documentation

---

## 🔄 Why This Project?
* This project demonstrates:
* Migration from legacy structure to modern architecture
* Clean separation of concerns
* Proper DbContext configuration
* Production-ready backend structure
* Professional Git workflow

---

## 📈 Future Improvements
* Authentication & Authorization (JWT)
* Role-based access control
* Docker support
* Unit & integration testing
* CI/CD pipeline
* Caching layer (Redis)

---

## 👨‍💻 Author

Ivan Rojas
This project has been built as part of portfolio modernization and backend specialization of my existing project "MercaditoAviario".

