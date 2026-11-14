# Pharmacy System — Agent Instructions

## Project overview
A single-pharmacy e-commerce system inspired by Chefaa.

## Technology stack
- .NET 10, ASP.NET Core Web API, EF Core, SQL Server, ASP.NET Core Identity, JWT
- React + TypeScript, Vite
- Three-tier architecture: Pharmacy.API (presentation), Pharmacy.BLL (business logic), Pharmacy.DAL (data access).

## Responsibilities
The developer implements backend business logic manually for educational purposes. Codex handles database implementation (entities, enums, DbContext, Fluent API configurations, constraints, indexes, Identity integration, migrations and seed data) and React frontend development when requested.

## Rules
- Do not implement services, repositories, DTOs, controllers or business logic unless explicitly requested.
- Keep the existing three-tier architecture simple; do not introduce Clean Architecture, CQRS, MediatR, DDD or unnecessary abstractions.
- Read docs/DatabaseDesign.md before database work. Consult the requirements and planning documents in docs/ where relevant.
- Report inconsistencies and unresolved requirements before implementing affected parts. Do not change approved database design without permission.
- Do not create migrations or update the database without approval.
- Do not commit or push without permission.
- Never hardcode credentials or secrets.
- Build and validate changes, and report changes, errors and unresolved issues.
- Do not start frontend implementation until explicitly requested.
