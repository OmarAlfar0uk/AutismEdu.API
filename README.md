<div align="center">

# 🧩 AutismEdu.API
### AI-Powered Educational & Behavioral Platform for Children with Autism Spectrum Disorder (ASD)

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Google Gemini AI](https://img.shields.io/badge/Google_Gemini_AI-4285F4?style=for-the-badge&logo=google&logoColor=white)](https://ai.google.dev/)
[![Architecture](https://img.shields.io/badge/Architecture-Vertical_Slices_%26_CQRS-blue?style=for-the-badge&logo=diagram-project&logoColor=white)](#-system-architecture)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellowgreen?style=for-the-badge)](LICENSE)
[![Author](https://img.shields.io/badge/Author-Omar%20Alfarouk-orange?style=for-the-badge&logo=github&logoColor=white)](https://github.com/OmarAlfar0uk)

<p align="center">
  <a href="#-key-features">Key Features</a> •
  <a href="#-system-architecture">System Architecture</a> •
  <a href="#-tech-stack">Tech Stack</a> •
  <a href="#-project-structure">Project Structure</a> •
  <a href="#-getting-started">Getting Started</a> •
  <a href="#-api-reference">API Reference</a> •
  <a href="#-author">Author</a>
</p>

</div>

---

## 📌 Executive Overview

**AutismEdu.API** is an enterprise healthcare and specialized educational platform designed to empower parents, specialists, and educators supporting children on the Autism spectrum. The system bridges behavioral psychology and modern AI by offering tailored interactive learning activities, milestone tracking, and an intelligent **Google Gemini-powered ChatBot** that delivers contextual guidance and developmental assessments.

> [!NOTE]
> Engineered using **Vertical Slice Architecture** with **CQRS (Command Query Responsibility Segregation)**, **MediatR**, and **FluentValidation**, isolating feature logic to ensure maximum maintainability, scalability, and testability.

---

## ✨ Key Features

| ⚡ Feature | 💡 Description | 🛠 Engineering & Domain Detail |
|---|---|---|
| **🤖 Gemini AI Assistant** | Intelligent chatbot delivering guidance for parents and therapists | Integrated with Google Gemini API via `IGeminiService` with contextual memory |
| **🎯 Activity Engine** | Categorized developmental exercises and interactive challenges | Custom cognitive difficulty curves and real-time progress recording |
| **👶 Child Profile Tracking** | Comprehensive tracking of behavioral milestones and goals | Role-based parent/specialist authorization via `ChildAuthorizationService` |
| **🔐 Secure Authentication** | Robust RBAC for Parents, Specialists, and Administrators | ASP.NET Identity + Stateless JWT Bearer tokens + Refresh Token rotation |
| **📧 Automated Notifications** | Email verifications, password resets, and progress updates | Asynchronous email dispatch using MailKit with HTML templating |
| **🧪 Automated Unit Testing** | High-coverage authorization and domain tests | xUnit test suite with Moq for critical security and domain rules |

---

## 🏛 System Architecture

The solution implements **Vertical Slice Architecture**, organizing code by feature cohesion rather than arbitrary technical layers:

```mermaid
flowchart TD
    subgraph Clients["🖥️ Clients & Frontends"]
        Mobile["📱 Mobile App (Parents & Specialists)"]
        Web["💻 Specialist & Admin Web Portal"]
    end

    subgraph API["🚪 AutismEdu.API (Vertical Slices)"]
        AuthFeature["🔐 Auth Slice<br/>(Login, Register, Refresh Tokens)"]
        ChatbotFeature["🤖 ChatBot Slice<br/>(AskQuestion, Conversation Context)"]
        ChildrenFeature["👶 Children Slice<br/>(Profiles, Milestones, Auth Guard)"]
        ActivityFeature["🎨 Activities Slice<br/>(Exercises, Progress Analytics)"]
    end

    subgraph External["🌐 External Integrations"]
        Gemini["✨ Google Gemini AI Service"]
        Mail["📧 MailKit SMTP Email Server"]
        Storage["📁 File & Media Storage Service"]
    end

    subgraph Database["🗄️ Persistence Tier"]
        SQL[("Microsoft SQL Server<br/>(EF Core + Migrations)")]
    end

    Clients --> API
    ChatbotFeature --> Gemini
    AuthFeature --> Mail
    ActivityFeature --> Storage
    API --> SQL
```

---

## ⚡ Tech Stack

| Category | Technology | Purpose |
|---|---|---|
| **Framework & Runtime** | ![.NET 8](https://img.shields.io/badge/.NET_8-512BD4?style=flat-square&logo=dotnet&logoColor=white) ![C#](https://img.shields.io/badge/C%23_12-239120?style=flat-square&logo=csharp&logoColor=white) | Core backend runtime and modern language features |
| **AI Integration** | ![Gemini](https://img.shields.io/badge/Google_Gemini-4285F4?style=flat-square&logo=google&logoColor=white) | Generative behavioral guidance and diagnostic suggestions |
| **Design Pattern** | ![Vertical Slice](https://img.shields.io/badge/Vertical_Slice-Architecture-blue?style=flat-square) ![CQRS](https://img.shields.io/badge/CQRS-MediatR-blueviolet?style=flat-square) | Feature encapsulation and command/query mediation |
| **Data & ORM** | ![EF Core](https://img.shields.io/badge/EF_Core-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white) ![SQL Server](https://img.shields.io/badge/MS_SQL_Server-CC292B?style=flat-square&logo=microsoftsqlserver&logoColor=white) | Code-First migrations and relational data persistence |
| **Validation & Mapping** | ![FluentValidation](https://img.shields.io/badge/FluentValidation-Pipeline-brightgreen?style=flat-square) | Pipeline request validation and model safety |
| **Testing** | ![xUnit](https://img.shields.io/badge/xUnit-Tests-blueviolet?style=flat-square) ![Moq](https://img.shields.io/badge/Moq-Mocking-orange?style=flat-square) | Unit testing for business logic and authorization services |

---

## 📂 Project Structure

```text
AutismEdu.API/
├── AutismEdu.API/
│   ├── Contracts/                   # Service abstractions (IGeminiService, ITokenService, etc.)
│   ├── Data/                        # EF Core ApplicationDbContext & Migrations
│   ├── Features/                    # Vertical Slices by business capability
│   │   ├── Activities/              # Activity management & tracking endpoints
│   │   ├── Auth/                    # Registration, Login, Token Refresh, Admin onboarding
│   │   ├── ChatBot/                 # Gemini AI integration handlers & DTOs
│   │   ├── ChildActivity/           # Activity completion & scoring
│   │   └── Children/                # Child profiles, assignments & guardians
│   ├── Services/                    # External service implementations (Gemini, MailKit)
│   └── Program.cs                   # Dependency injection & middleware pipeline
├── AutismEdu.API.Tests/             # Unit and integration test suite
│   ├── ChildAuthorizationServiceTests.cs
│   └── AutismEdu.API.Tests.csproj
└── AutismEdu.API.sln                # Visual Studio Solution
```

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Docker container)

### Installation & Run

1. **Clone the repository:**
   ```bash
   git clone https://github.com/OmarAlfar0uk/AutismEdu.API.git
   cd AutismEdu.API
   ```

2. **Restore Dependencies & Build:**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Run the Application:**
   ```bash
   dotnet run --project AutismEdu.API
   ```
   Navigate to `http://localhost:5000/swagger` to explore the interactive OpenAPI documentation.

4. **Run the Tests:**
   ```bash
   dotnet test
   ```

---

## 📖 API Reference

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| `POST` | `/api/Auth/Register` | Register a new parent or guardian | No |
| `POST` | `/api/Auth/Login` | Authenticate and obtain JWT + Refresh token | No |
| `POST` | `/api/Auth/RefreshToken` | Refresh an expired access token | No |
| `POST` | `/api/ChatBot/AskQuestion` | Submit a query to the Gemini AI assistant | Yes (Bearer) |
| `GET` | `/api/Children` | Retrieve all children associated with caller | Yes (Bearer) |
| `POST` | `/api/Children` | Register a new child profile | Yes (Bearer) |
| `GET` | `/api/Activities` | List curated educational activities | Yes (Bearer) |
| `POST` | `/api/ChildActivity/Submit` | Record completion and feedback for an exercise | Yes (Bearer) |

---

## 👨‍💻 Author

**Omar Alfarouk**  
*Full-Stack .NET & Software Engineer*  

- 🌐 **GitHub:** [@OmarAlfar0uk](https://github.com/OmarAlfar0uk)
- 💼 **LinkedIn:** [omar-alfarouk](https://www.linkedin.com/in/omar-alfarouk-252471251/)
- 📧 **Email:** [omaralfarouk646@gmail.com](mailto:omaralfarouk646@gmail.com)

---

<div align="center">
  <sub>Built with ❤️ by Omar Alfarouk. Licensed under the <a href="LICENSE">MIT License</a>.</sub>
</div>
