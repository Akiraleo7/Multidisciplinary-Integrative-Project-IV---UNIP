# HEJ Care Suite - Final Validation Runbook

Run this checklist in the Codespace using the merged `main` branch. Save the
terminal output and screenshots for the report and presentation.

## 1. Update and verify source

```bash
git fetch origin
git switch main
git pull --ff-only origin main
git status
```

The working tree should be clean and the branch should contain the merged MVP.

## 2. Validate application and frontend

```bash
dotnet clean
dotnet build
dotnet test
node --check src/HEJCareSuite.Api/wwwroot/app.js
```

Record the final test count and successful exit codes.

## 3. Validate database

In SSMS, confirm that scripts `001` through `005` were applied and then run:

```text
database/006_validation_queries.sql
```

Save results showing schema versions `1` through `5`, core tables, constraints,
indexes, triggers, row counts, and audit records.

## 4. Run the application

Inside the Codespace, use the repository's existing Dev Container and SQL
Server configuration. Confirm the application connection string points to the
Codespace SQL Server and that `MSSQL_SA_PASSWORD` is supplied as a secret.

Then open the forwarded application URL and verify:

1. `GET /health` returns HTTP 200.
2. Login succeeds with the local demonstration account:
   - Email: `admin@hejcare.local`
   - Password: `ChangeMe123!`
3. Patient create, list, edit, and delete work.
4. Triage registration and history work.
5. Admission, bed allocation, history, and discharge work.
6. Logout blocks protected operations.

`MSSQL_SA_PASSWORD` is the SQL Server `sa` password and is not the
application login password. The demonstration account is inserted by
`database/003_authentication_schema.sql`. If that account already existed with
another password, the idempotent script does not overwrite it; verify the
account in SSMS before the presentation.

## 5. Capture final evidence

- One screenshot of the running application and health response.
- Screenshots of each completed workflow.
- Terminal output for build and tests.
- SSMS output from script `006`.
- GitHub Actions link showing green CI.
- Pull Request links for the completed increments.

Do not include passwords, password hashes, salts, or connection strings in
screenshots or the academic report.
