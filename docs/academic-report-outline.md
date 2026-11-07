# PIM IV Academic Report Outline

This outline is a controlled source for the written report. It follows the
project documentation language standard (English) and leaves institution,
student, date, and measurement fields explicit so that no academic evidence is
fabricated.

## Front matter

### Cover page

- Institution: `[fill with official institution name]`
- Course: `[fill with official course name]`
- Project title: `HEJ Care Suite - Hospital Edward Jenner Digital Transformation Platform`
- Authors: `[fill with the six team members]`
- City and year: `[fill with official submission data]`

### Title page

Use the institution's required ABNT wording for the project type, advisor, and
course. Do not copy a template from another institution without checking the
official academic guide.

### Abstract

This project presents HEJ Care Suite, a hospital operations MVP developed for
PIM IV. The implemented release centralizes patient registration, emergency
triage, and hospitalization admission and discharge through a responsive web
interface and versioned REST API. The solution uses ASP.NET Core on .NET 8,
SQL Server, MVC presentation, and Clean Architecture boundaries. Automated
tests, CI validation, SQL audit triggers, container packaging, and a health
smoke test support reproducibility. Exams, pharmacy, operational dashboards,
and native mobile delivery remain planned extensions.

**Keywords:** hospital information system; REST API; Clean Architecture;
SQL Server; ASP.NET Core; healthcare operations.

## 1 Introduction

Describe the operational problem: fragmented hospital workflows, manual
handoffs, and limited traceability. Introduce HEJ Care Suite as the proposed
modular platform and delimit the report to the presentation MVP.

Evidence: `README.md`, `docs/requirements-traceability.md`.

## 2 Problem, justification, and objectives

### 2.1 Problem

Explain how disconnected patient intake, triage, and bed management can delay
operations and reduce visibility.

### 2.2 General objective

Design and implement a functional hospital operations MVP that centralizes
patient, triage, and hospitalization workflows.

### 2.3 Specific objectives

1. Implement authenticated patient management.
2. Record emergency triage and risk classification.
3. Manage admission, bed allocation, history, and discharge.
4. Persist operational data in SQL Server with audit records.
5. Validate the solution with automated tests and CI.
6. Package the application in a deployable container.

## 3 Theoretical foundation

Cover only concepts actually used:

- MVC and layered/Clean Architecture;
- REST and HTTP status contracts;
- relational modeling, constraints, indexes, and triggers;
- authentication sessions and password hashing;
- automated testing and continuous integration;
- containerization and deployment smoke tests.

Add authoritative references required by the institution. Candidate technical
sources must be verified before inclusion.

## 4 Methodology

Describe Scrum-oriented incremental delivery, vertical slices, Pull Requests,
Codespaces, SQL Server/SSMS validation, and automated CI. Include the actual
team workflow and sprint data supplied by the team; do not infer Jira keys or
sprint numbers from Git history.

Evidence: `docs/development-log.md`, Pull Requests, Jira, and Confluence.

## 5 Requirements and traceability

Use `docs/requirements-traceability.md` as the requirements table. Explain
that FR-01, FR-02, FR-03, the REST baseline, authentication baseline, audit
trail, CI, container packaging, and health smoke test are in the presentation
release. Clearly identify planned extensions as limitations.

## 6 Architecture and modeling

Include:

- dependency direction:

  ```text
  API -> Infrastructure -> Application -> Domain
  ```

- conceptual entity model: Patient, UserAccount, Triage, HospitalAdmission,
  AuditLog, and SchemaVersion;
- database scripts `001` through `005`;
- audit triggers and filtered active-record indexes;
- API and web delivery structure.

Evidence: `docs/architecture-and-decisions.md`, SQL scripts, and the final ERD.

## 7 Implementation

Present the implemented vertical slices:

1. patient CRUD;
2. authentication and logout;
3. emergency triage;
4. hospitalization admission and discharge;
5. responsive web interface;
6. CI and container delivery.

Use `docs/api-reference.md` for endpoint tables and request examples.

## 8 Tests and results

Record only measured results from the final release candidate:

| Check | Result | Evidence |
| --- | --- | --- |
| `dotnet build` | `[fill with Codespace result]` | terminal output |
| `dotnet test` | `[fill with count and result]` | terminal output |
| Frontend syntax check | `[fill with result]` | CI log |
| Docker image build | `[fill with result]` | CI log |
| `GET /health` | `[fill with HTTP status]` | deployment smoke test |
| SQL validation script | `[fill with result]` | SSMS output |

## 9 Deployment and operation

Follow `docs/deployment-guide.md`. Include the target environment, connection
string configuration method, container port, database migration order, and
smoke test. Never include passwords, hashes, salts, or connection secrets.

## 10 Social responsibility, inclusion, and accessibility

Describe the intended inclusive audience and the responsive, readable web
interface. Report only accessibility checks that were actually performed.
List remaining accessibility work as future improvement.

## 11 Limitations and future work

State explicitly that the presentation MVP does not implement:

- laboratory/radiology exams;
- prescription and pharmacy dispensing;
- operational dashboards;
- native mobile application;
- complete physician confirmation workflow;
- post-presentation security hardening and production observability.

## 12 Conclusion

Summarize the delivered functional MVP, its architectural consistency,
traceability, automated validation, and reproducible deployment path. Do not
claim that planned modules are complete.

## References and appendices

Use the institution's ABNT reference and formatting rules. Suggested
appendices:

- API examples;
- SQL validation output;
- selected automated test output;
- screenshots of the demonstration flow;
- deployment commands;
- Pull Request and CI evidence.
