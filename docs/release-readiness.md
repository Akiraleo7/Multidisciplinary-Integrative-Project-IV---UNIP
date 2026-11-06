# HEJ Care Suite - Presentation Release Checklist

Use this checklist in the Codespace and in the final presentation environment.
It is intentionally limited to the functional MVP deadline.

## Automated checks

- [ ] PR check is green.
- [ ] `dotnet build` passes in the Codespace.
- [ ] `dotnet test` passes in the Codespace.
- [ ] `node --check src/HEJCareSuite.Api/wwwroot/app.js` passes.
- [ ] Docker image build passes.

## Database checks

- [ ] SQL scripts `001` through `005` are applied in the correct order.
- [ ] `dbo.SchemaVersion` contains versions `1` through `5`.
- [ ] `database/006_validation_queries.sql` completes without errors.
- [ ] `AuditLog` contains records after patient, triage, and admission actions.

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

## Deferred scope

Do not block the presentation release on exams, pharmacy, operational
dashboards, native mobile delivery, or post-presentation hardening. These items
remain documented backlog scope for the next cycle.
