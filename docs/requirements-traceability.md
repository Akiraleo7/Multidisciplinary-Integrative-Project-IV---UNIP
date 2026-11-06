# HEJ Care Suite - Requirements Traceability Matrix

This matrix records the current MVP implementation status and identifies the
evidence required for the final academic report. Jira issue keys and sprint
numbers must be filled with the project's actual Atlassian references.

## Functional requirements

| Requirement | Current status | Implementation evidence | Validation evidence | Jira / Sprint |
| --- | --- | --- | --- | --- |
| FR-01 - Register and manage patients | Implemented | `Patient`; `PatientsController`; `SqlPatientRepository`; responsive patient UI | Domain, Application, and API tests; CRUD walkthrough | To be filled |
| FR-02 - Register emergency triage and risk classification | Implemented | `Triage`; `TriageController`; `004_triage_schema.sql`; triage UI | Domain, Application, and API workflow validation | To be filled |
| FR-03 - Manage hospitalization admission, bed allocation, and discharge | Implemented | `HospitalAdmission`; `HospitalizationController`; `005_hospitalization_schema.sql`; hospitalization UI | Domain and Application tests; admission/discharge walkthrough | To be filled |
| FR-04 - Request and track lab/radiology exams | Planned | No exam aggregate, schema, API, or UI is included in the current MVP | Define backlog acceptance criteria before implementation | To be filled |
| FR-05 - Control medication dispensing through pharmacy workflow | Planned | No prescription or dispensing workflow is included in the current MVP | Define backlog acceptance criteria before implementation | To be filled |
| FR-06 - Provide role-based dashboards for operations | Planned | Authentication exposes a role, but operational dashboards are not implemented | Define dashboard indicators and role permissions | To be filled |
| FR-07 - Expose REST APIs for web/mobile integration | Partially implemented | Versioned REST endpoints under `/api/v1` support patients, triage, authentication, and hospitalization | API integration tests and endpoint walkthrough | To be filled |

## Non-functional requirements

| Requirement | Current status | Implementation evidence | Validation evidence | Jira / Sprint |
| --- | --- | --- | --- | --- |
| NFR-01 - Secure authentication and session control | Implemented baseline | PBKDF2 password hashing; SQL `UserAccount`; HTTP-only `hejcare.auth` cookie | Authentication integration tests and login/logout walkthrough | To be filled |
| NFR-02 - Audit trail for critical operations | Implemented baseline | `AuditLog`; SQL triggers for patients, triage, and hospitalization | SSMS queries showing audit records after CRUD operations | To be filled |
| NFR-03 - Responsive web interface and mobile usability | Partially implemented | Responsive web UI in `wwwroot`; no native mobile client | Browser validation at desktop and narrow viewport sizes | To be filled |
| NFR-04 - Availability suitable for hospital routine operations | Baseline only | Containerized development environment and `GET /health` smoke endpoint | Build, test, container, and deployment checks still required | To be filled |
| NFR-05 - Healthcare privacy-aligned data protection | Partially implemented | Secrets excluded from source; authenticated API; parameterized SQL | Security review and deployment configuration evidence required | To be filled |

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
