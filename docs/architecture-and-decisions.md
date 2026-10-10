# HEJ Care Suite - Architecture and Technical Decisions

## Purpose

This document records the technical decisions that guide the PIM IV implementation.
It is maintained as project evidence and as source material for the final academic report.

## Current stack

| Area | Decision |
| --- | --- |
| Development environment | GitHub Codespaces with Dev Containers |
| Backend | ASP.NET Core on .NET 8 |
| Database | SQL Server 2022 Developer Edition on Linux |
| Database client | SSMS through an SSH tunnel |
| API style | REST with versioned routes under `/api/v1` |
| Architecture | MVC presentation with layered/Clean Architecture boundaries |
| Persistence | `Microsoft.Data.SqlClient` in the Infrastructure layer |
| Source control | Git feature branch plus Pull Request to `main` |

## Dependency direction

The dependency direction is inward:

```text
API -> Infrastructure -> Application -> Domain
```

- **Domain** contains entities and business invariants without framework or database
  dependencies.
- **Application** contains use cases and repository contracts.
- **Infrastructure** implements database access and external technical concerns.
- **API** exposes HTTP endpoints and composes the application.
- **API contracts** use response DTOs so HTTP payloads do not expose domain entities
  directly.

## Decisions and rationale

### GitHub Codespaces

Codespaces was selected because the local Windows host does not have the .NET SDK or
Docker installed. The Dev Container provides a reproducible .NET development environment
and runs SQL Server as a separate Compose service.

### SQL Server as a separate service

SQL Server runs in the `sqlserver` Compose service, while the .NET SDK runs in the `app`
service. The services communicate through the Docker network using the hostname
`sqlserver`. This keeps the application and database responsibilities isolated.

### Secret handling

`MSSQL_SA_PASSWORD` is supplied as a GitHub Codespaces secret and is consumed by Docker
Compose. Passwords are not stored in source files, commits, or the final report.

### Initial persistence strategy

The first API baseline used an in-memory repository to validate the HTTP flow. Once the
SQL Server schema was created and tested through SSMS, `SqlPatientRepository` replaced
the in-memory implementation. This preserves the Application contract while changing
only the Infrastructure adapter.

### SSMS access

The Codespaces port forwarding command was insufficient for the SQL Server container
network in this setup. An SSH tunnel was enabled in the Dev Container and is used as:

```powershell
gh codespace ssh --codespace NOME_DO_CODESPACE -- -N -L 1433:sqlserver:1433
```

SSMS then connects to `tcp:127.0.0.1,1433`. The tunnel must remain open while SSMS is
connected.

## Current scope and next decisions

- Patient registration and listing are the first implemented feature.
- The initial SQL schema includes `Patient`, `AuditLog`, and `SchemaVersion`.
- Authentication, authorization, triage, admissions, exams, pharmacy, and mobile
  workflows remain future increments.
- Database migrations and automated integration tests should be introduced before
  production deployment.
