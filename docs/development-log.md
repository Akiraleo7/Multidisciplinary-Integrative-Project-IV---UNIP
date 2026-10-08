# HEJ Care Suite - Development Log

This log records the implementation sequence, evidence, and validation results. New
increments must add an entry instead of relying only on chat history.

## 2026-10-07 - Foundation and environment

- Reviewed the PIM IV scope and MVP requirements in `README.md`.
- Created the initial .NET solution with Domain, Application, Infrastructure, and API
  projects.
- Added the first patient registration use case and `GET`/`POST /api/v1/patients`.
- Configured GitHub Codespaces with a .NET 8 `app` service and SQL Server 2022
  `sqlserver` service.
- Moved the SQL Server password to the `MSSQL_SA_PASSWORD` Codespaces secret.
- Corrected the initial invalid Dev Container image reference.

## 2026-10-07 - Codespaces and SSMS validation

- Recovered the Codespace after identifying the missing `MSSQL_SA_PASSWORD` variable.
- Confirmed the API endpoint returned HTTP 200 and an empty JSON array (`[]`).
- Added the Dev Container SSHD feature to support local SQL tunneling.
- Connected SSMS through `127.0.0.1:1433` using the SSH tunnel.
- Validated the database server with `SELECT @@VERSION`, confirming SQL Server 2022
  Developer Edition on Ubuntu 22.04.5.

## 2026-10-07 - Database baseline

- Added `database/001_initial_schema.sql`.
- Created `HEJCareSuite`, `Patient`, `AuditLog`, and `SchemaVersion`.
- Added patient constraints, a unique document index, and the patient audit trigger.
- Executed and validated the script successfully in SSMS.

## 2026-10-07 - SQL persistence

- Added `SqlPatientRepository` with parameterized ADO.NET commands.
- Configured the API to read `ConnectionStrings__SqlServer` from the app container.
- Kept the SQL password outside the repository.
- Published the implementation in a Pull Request for integration into `main`.

## Evidence and reproducibility

The following evidence should be retained for the final report:

1. Codespaces creation and recovery log, with secrets redacted.
2. Dev Container and Docker Compose configuration.
3. SSMS connection and `SELECT @@VERSION` result.
4. Initial schema script and successful execution result.
5. API requests and responses for patient registration and listing.
6. Pull Request and commit history for each coherent increment.
