# HEJ Care Suite - Presentation Release Checklist

Use this checklist in the Codespace and in the final presentation environment.
It is intentionally limited to the functional MVP deadline.

## Automated checks

- [x] PR check is green.
- [x] `dotnet build` passes in the Codespace.
- [x] `dotnet test` passes in the Codespace.
- [x] `node --check src/HEJCareSuite.Api/wwwroot/app.js` passes.
- [x] Docker image build passes.

## Database checks

- [x] SQL scripts `001` through `005` are applied in the correct order.
- [x] `dbo.SchemaVersion` contains versions `1` through `5`.
- [x] `database/006_validation_queries.sql` completes without errors.
- [x] `AuditLog` contains records after patient, triage, and admission actions.

## Demonstration flow

1. Open the deployed application.
2. Call `GET /health` and show HTTP 200.
3. Log in with the local demonstration account.
4. Register, edit, list, and delete a patient.
5. Register a triage record and show its history.
6. Admit a patient to a bed and show admission history.
7. Discharge the patient with mandatory notes.
8. Show the related audit records in SSMS.
9. Log out and show that the authenticated area is no longer available.

The complete flow was validated in the Codespace, including patient creation
and removal, bed operations, authentication, and the corresponding audit
records. The validation used the existing database and did not require
deleting or recreating it.

## Deferred scope

Do not block the presentation release on exams, pharmacy, operational
dashboards, native mobile delivery, or post-presentation hardening. These items
remain documented backlog scope for the next cycle.
