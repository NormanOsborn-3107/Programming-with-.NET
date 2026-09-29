# 🎓 Programming with .NET — PRN232

> **Course**: PRN232 — Building Cross-Platform Back-End Application With .NET  
> **Student**: Trần Anh Kiệt — SE194832 — Class SE1917  
> **Semester**: Fall 2025 | FPT University

---

## 📌 Project Overview

This repository contains lab assignments and projects developed during the **PRN232** course at FPT University. The course focuses on building modern, cross-platform back-end applications using **.NET 8**, **ASP.NET Core Web API**, **Entity Framework Core**, and **Docker**.

---

## 📁 Repository Structure

```
Programming-with-.NET/
│
├── ProjectDotNet/          # Lab 1 — ASP.NET Core starter project
│   ├── Controllers/        # API Controllers
│   ├── Program.cs          # Entry point
│   └── appsettings.json    # Configuration
│
├── PRN232.LMS.API/         # Main LMS REST API (Lab assignment)
├── PRN232.LMS.Repositories/# Data Access Layer (Repository pattern)
├── PRN232.LMS.Services/    # Business Logic Layer
│
├── Dockerfile              # Docker image build config
├── docker-compose.yml      # Multi-container setup (API + SQL Server)
└── README.md
```

---

## 🚀 Projects

### 1. ProjectDotNet — ASP.NET Core Starter
A basic ASP.NET Core Web API project used as an introduction to:
- RESTful API design with ASP.NET Core
- Controller-based routing
- Dependency Injection

### 2. PRN232 LMS — Learning Management System API
A full-featured **Learning Management System REST API** built with:
- **ASP.NET Core 8** Web API
- **Entity Framework Core** (Code-First with Migrations)
- **SQL Server 2022** (via Docker)
- **Repository & Service** pattern (3-layer architecture)
- **Docker** containerization

#### 🗂️ Data Model
| Entity | Description |
|--------|-------------|
| Semester | Academic semesters |
| Subject | Subjects offered per semester |
| Course | Courses within subjects |
| Student | Student records |
| Enrollment | Student–Course enrollment mapping |

#### 📊 Seeded Data
| Table | Rows |
|-------|------|
| Semester | 5 |
| Subject | 10 |
| Course | 20 |
| Student | 50 |
| Enrollment | 500 |

> Seeding is **idempotent** — restarting containers will NOT duplicate rows (EF Core `HasData` migrations).

---

## ⚙️ How to Run

### Option A — Local Development

```bash
# Run the LMS API locally
dotnet run --project PRN232.LMS.API

# Access points:
# API:     http://localhost:5062
# Swagger: http://localhost:5062/swagger
```

### Option B — Docker (Recommended)

```bash
# Start all services (API + SQL Server)
docker compose up --build -d

# Access points:
# API:     http://localhost:8080
# Swagger: http://localhost:8080/swagger
# Health:  http://localhost:8080/health
```

---

## 🔌 API Endpoints

| Resource | Endpoints |
|----------|-----------|
| Semesters | `GET /api/semesters` · `GET /api/semesters/{id}` · `POST` · `PUT` · `DELETE` |
| Subjects | `GET /api/subjects` · `GET /api/subjects/{id}` · `POST` · `PUT` · `DELETE` |
| Courses | `GET /api/courses` · `GET /api/courses/{id}` · `POST` · `PUT` · `DELETE` |
| Students | `GET /api/students` · `GET /api/students/{id}` · `POST` · `PUT` · `DELETE` |
| Enrollments | `GET /api/enrollments` · `GET /api/enrollments/{id}` · `POST` · `PUT` · `DELETE` |

### 🔍 Query Parameters (Students)

| Parameter | Example | Description |
|-----------|---------|-------------|
| `search` | `?search=nguyen` | Case-insensitive name search |
| `sort` | `?sort=fullName,-dateOfBirth` | Comma-separated, `-` = descending |
| `page` + `size` | `?page=1&size=10` | 1-based paging, max size 100 |
| `fields` | `?fields=studentId,fullName` | Sparse field selection |
| `expand` | `?expand=enrollments` | Expand related navigation data |

---

## 🛠️ Tech Stack

| Category | Technology |
|----------|-----------|
| Language | C# / .NET 8 |
| Framework | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core 8 |
| Database | SQL Server 2022 |
| Containerization | Docker + Docker Compose |
| API Docs | Swagger / OpenAPI |
| Architecture | Repository + Service (3-layer) |

---

## 📄 License

This project is for **educational purposes** only as part of the PRN232 course at FPT University.
