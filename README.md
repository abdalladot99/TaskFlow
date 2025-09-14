# TaskFlow – Smart Task Management System 🚀

![.NET](https://img.shields.io/badge/.NET-8-blue) ![C#](https://img.shields.io/badge/C%23-Programming-blue) ![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-green) ![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-red)

## 📖 Overview
**TaskFlow** is a comprehensive multi-user task management system built with **.NET 8** and **ASP.NET Core Web API**.  
The project is designed using **Clean Architecture** and **CQRS** for scalability, maintainability, and separation of concerns.  
It provides a robust platform for creating tasks, collaborating with other users, receiving real-time notifications, and managing work efficiently.

## 🌟 Key Features

### 🔐 Authentication & Authorization
- Secure login with **ASP.NET Identity** and **JWT**.
- Role-Based Access Control (**RBAC**) and **Custom Claims**.

### 📋 Task Management
- Create, edit, delete, and categorize tasks.
- Supports task **priorities**, **recurrence**, and **status tracking**.

### 🤝 Collaboration
- Share tasks with other users.
- Send and receive collaboration invitations.
- Accept or reject collaboration requests.

### 🔔 Notification System
- Real-time notifications for task updates and deadlines.
- Email notifications for new tasks, invitations, and reminders.
- In-app notifications for task assignments and updates.

### 📧 Email Integration
- Automatically sends task assignments and collaboration updates.

### 🗄️ Database Integration
- Uses **Entity Framework Core** with **SQL Server** for data persistence.

## 🛠️ Technologies & Tools
- **Languages:** C#, JavaScript, SQL
- **Frameworks & Libraries:** ASP.NET Core Web API, ASP.NET Identity, Entity Framework Core, LINQ, AutoMapper, MediatR
- **Architectural Patterns:** Clean Architecture, CQRS, Repository Pattern, Dependency Injection, SOLID Principles
- **Web Technologies:** HTML5, CSS3, Bootstrap, RESTful APIs
- **Database:** Microsoft SQL Server
- **Tools:** Visual Studio, Git, GitHub, NuGet, Postman
- **Security:** JWT Authentication, OAuth2, HTTPS/SSL, Email Verification, Password Reset via Email


## 🔗 Project Links

- 🎥 **Project Demo :** [ Go Demo ](https://taskflowapp.runasp.net/swagger/index.html)  
- 💼 **Project Post on LinkedIn:** [View on LinkedIn](##)



## 🏗️ Project Architecture
```plaintext
TaskFlow
│
├── TaskFlow.API          # RESTful Endpoints
├── TaskFlow.Application  # CQRS Handlers & Business Logic
├── TaskFlow.Core         # Entities & Core Business Rules
└── TaskFlow.Infrastructure # Database Access, Authentication, Notifications, Email
