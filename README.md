# EF_Library (Librarian Management System)

A C# and Entity Framework Core database project designed for managing library data structures, built as part of the learning track at Hillel IT School.

---

## 🚀 Project Overview
* **Language & Tech:** C# | .NET 8.0+ | Entity Framework Core
* **Approach:** Code-First Database Design
* **Architecture:** Multi-project solution dividing core application logic, data access, and models.

---

## 📂 Architecture & Project Structure

The solution consists of three main decoupled projects to ensure a clean separation of concerns:

* 📁 **`Library.App`** — The executable application layer. Handles configuration, dependency injection, runtime execution, and main logic.
* 📁 **`Library.DAL`** *(Data Access Layer)* — Contains the database context (`LibraryContext`), entity data configurations, and EF Core Migrations tracking the schema state.
* 📁 **`Library.Common`** — Shared data layer holding domain entity models (`Book`, `Author`, `User`, `BorrowedBook`, etc.) and system-wide utilities.

---

## 🛠️ Database Features Implemented
* Dynamic relationship mapping (Many-to-Many configurations for books and authors).
* Data integrity tracking through automated EF Core migrations.
* Historical records simulation using entity models for borrowed books.

---

## 💻 How to Build & Run Locally

1. Ensure you have the latest .NET SDK and an IDE that supports the new `.slnx` solution format (Visual Studio 2022 v17.10+ or JetBrains Rider).
2. Clone the repository:
   ```bash
   git clone https://github.com
   ```
3. Open `Library.slnx` inside your IDE.
4. Apply migrations to generate your local database instance:
   ```bash
   dotnet ef database update --project Library.DAL --startup-project Library.App
   ```
5. Run the `Library.App` startup project.
