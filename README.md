# 🎓 University Management System API

## 📖 Overview
A robust, scalable, and enterprise-grade Web API built to manage core university operations. Developed using **.NET 8** and **C#**, this system strictly adheres to **Clean Architecture** principles and SOLID design patterns to ensure maximum maintainability, testability, and high performance.

## 🏗️ Architecture & Technologies
* **Framework:** ASP.NET Core Web API (.NET 8)
* **Database:** SQL Server & Entity Framework Core (Code-First Approach)
* **Architecture:** Clean Architecture
* **Design Patterns:** Generic Repository, Unit of Work, Dependency Injection
* **Security:** JWT (JSON Web Token) Authentication & Role-Based Access Control (Admin, Professor, Student)
* **Data Mapping:** AutoMapper

## ✨ Key Features
* **Secure Authorization:** Role-based access control protecting sensitive endpoints.
* **Advanced Data Handling:** Implementation of Global Query Filters for **Soft Delete** mechanisms.
* **Optimized Retrievals:** Server-side pagination and data shaping for efficient data fetching.
* **Automated Data Seeding:** Built-in `DbInitializer` to automatically populate default roles, system admins, departments, and rooms upon startup.

## 🚀 API Documentation & Testing (Active Development)

> **⚠️ Development Notice:** 
> The current API endpoints represent the foundational layer of the system. **I am continuously developing, expanding, and optimizing these endpoints.** Future updates will introduce more complex business logic, advanced data relationships, and enhanced performance strategies.

To explore, interact with, and test the current capabilities of the API, two fully configured environments are provided:

### 1. Swagger UI
Swagger is fully integrated into the project. Simply run the application in the Development environment, and Swagger UI will automatically launch, providing an interactive, auto-generated documentation interface for all available endpoints and DTO schemas.

### 2. Postman Collection
A meticulously organized **Postman Collection** is available for this project. It is designed for smart and automated testing, featuring:
* Pre-configured environment variables (e.g., `{{baseUrl}}`).
* **Automated JWT Handling:** Test scripts are written in the Login/Register endpoints to automatically extract the generated Bearer token and apply it globally to all secured requests in the collection.

## 🛠️ Getting Started
1. Clone this repository to your local machine.
2. Update the `DefaultConnection` string in `appsettings.json` to point to your local SQL Server instance.
3. Open the Package Manager Console (PMC) and run `Update-Database` (or `dotnet ef database update` via CLI) to apply pending migrations.
4. Run the application. The system will automatically create the database and seed the initial required data.
