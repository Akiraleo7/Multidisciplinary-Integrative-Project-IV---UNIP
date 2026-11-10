# HEJ Care Suite - Requirements Traceability Matrix

This matrix records the current MVP implementation status and identifies the
evidence required for the final academic report. Jira and Confluence were part
of the original process plan but were not used as active project tools; Git,
Pull Requests, Codespaces, SSMS, and CI are the actual evidence sources.

## Functional requirements

| Requirement | Current status | Implementation evidence | Validation evidence | Jira / Sprint |
| --- | --- | --- | --- | --- |
| FR-01 - Register and manage patients | Implemented | `Patient`; `PatientsController`; `SqlPatientRepository`; responsive patient UI | Domain, Application, and API tests; CRUD walkthrough | Git/PR evidence |
| FR-02 - Register emergency triage and risk classification | Implemented | `Triage`; `TriageController`; `004_triage_schema.sql`; triage UI | Domain, Application, and API workflow validation | Git/PR evidence |
| FR-03 - Manage hospitalization admission, bed allocation, and discharge | Implemented | `HospitalAdmission`; `HospitalizationController`; `005_hospitalization_schema.sql`; hospitalization UI | Domain and Application tests; admission/discharge walkthrough | Git/PR evidence |
| FR-04 - Request and track lab/radiology exams | Planned | No exam aggregate, schema, API, or UI is included in the current MVP | Deferred after presentation | Backlog only |
| FR-05 - Control medication dispensing through pharmacy workflow | Planned | No prescription or dispensing workflow is included in the current MVP | Deferred after presentation | Backlog only |
| FR-06 - Provide role-based dashboards for operations | Planned | Authentication exposes a role, but operational dashboards are not implemented | Deferred after presentation | Backlog only |
| FR-07 - Expose REST APIs for web/mobile integration | Implemented baseline | Versioned REST endpoints under `/api/v1` support patients, triage, authentication, and hospitalization | API integration tests and endpoint walkthrough; mobile client remains future consumer | Git/PR evidence |

## Non-functional requirements

| Requirement | Current status | Implementation evidence | Validation evidence | Jira / Sprint |
| --- | --- | --- | --- | --- |
| NFR-01 - Secure authentication and session control | Implemented baseline | PBKDF2 password hashing; SQL `UserAccount`; HTTP-only `hejcare.auth` cookie | Authentication integration tests and login/logout walkthrough | Git/CI evidence |
| NFR-02 - Audit trail for critical operations | Implemented baseline | `AuditLog`; SQL triggers for patients, triage, and hospitalization | SSMS queries showing audit records after CRUD operations | SSMS evidence |
| NFR-03 - Responsive web interface and mobile usability | Implemented baseline | Responsive web UI, mobile navigation drawer, installable PWA manifest, cached application shell, and shared REST APIs | Browser validation at desktop/mobile viewport and PWA installation evidence | Screenshot/PWA evidence |
| NFR-04 - Availability suitable for hospital routine operations | Implemented baseline | Containerized application, CI build/test gate, and `GET /health` smoke endpoint | GitHub Actions build/test/container checks; deployment smoke test | CI/deploy evidence |
| NFR-05 - Healthcare privacy-aligned data protection | Partially implemented | Secrets excluded from source; authenticated API; parameterized SQL | Deployment configuration evidence; hardening deferred | Code/config evidence |

## Business rules

| Rule | Current status | Implementation evidence | Gap |
| --- | --- | --- | --- |
| BR-01 - Emergency triage before non-critical outpatient processing | Not implemented | Triage can be registered and queried | Outpatient workflow and enforcement rule are not part of the MVP |
| BR-02 - Prescription required before medication dispensing | Not implemented | No prescription or pharmacy workflow exists | Requires prescription and dispensing modules |
| BR-03 - Final physician confirmation before discharge | Partially implemented | Discharge requires mandatory notes and active admission status | Physician identity and explicit confirmation are not yet modeled |

## Evidence checklist

The final report should attach or reference:

1. Build and test output from the Codespace.
2. Screenshots of login, patient CRUD, triage, admission, and discharge.
3. SSMS queries showing schema versions, constraints, indexes, triggers, and audit records.
4. API request/response examples for each implemented endpoint.
5. Pull Requests and commits associated with each completed vertical slice.

## Presentation scope decision

The presentation release is the functional MVP composed of FR-01, FR-02, FR-03,
the authentication baseline, audit trail, responsive web interface, REST API,
CI, container packaging, and deployment smoke testing. FR-04, FR-05, FR-06,
native mobile delivery, and the complete BR-01/BR-02/BR-03 workflows are
documented as planned or partial scope and must not be presented as completed.
