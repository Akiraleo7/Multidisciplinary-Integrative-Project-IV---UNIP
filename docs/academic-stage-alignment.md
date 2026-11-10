# Academic Stage Alignment - HEJ Care Suite

This document is the evidence map for the ABNT report. It uses the repository
implementation as the source of truth and does not claim deliverables that were
not built.

## Stage 4 - Web solution

**Implemented.** The ASP.NET Core API serves the responsive web application
from `src/HEJCareSuite.Api/wwwroot`. The solution uses versioned REST endpoints,
cookie authentication, parameterized SQL repositories, audit triggers, and a
sidebar-based administrative workspace with dedicated patient, triage, and
admission screens.

Evidence: `src/HEJCareSuite.Api/Program.cs`, `Controllers/`, `wwwroot/`,
`docs/api-reference.md`, `.github/workflows/ci.yml`.

## Stage 5 - Mobile solution

**Implemented as mobile web/PWA support.** The same application is responsive
at narrow viewports, provides a mobile navigation drawer, exposes a web app
manifest, can be installed as a standalone PWA, and caches the application
shell for resilient startup. Mobile clients use the same `/api/v1` REST
contracts and cookie session as the web interface.

This is not a separate native Android/iOS binary. The academic demonstration
should present the installed PWA in a mobile viewport and explain that native
distribution remains a future packaging option.

Evidence: `wwwroot/manifest.webmanifest`, `wwwroot/sw.js`, responsive rules in
`wwwroot/styles.css`, and the REST contract in `docs/api-reference.md`.

## VLibras accessibility integration

**Implemented.** The official VLibras widget is loaded in the web shell through
the public VLibras plugin. It is available on the login and authenticated
screens when the browser has internet access. This integration is an external
accessibility service and is not part of the SQL Server or API data model.

Evidence: the VLibras widget markup and script in
`src/HEJCareSuite.Api/wwwroot/index.html`.

## Stage 6 - Software architecture

**Implemented baseline.** The code follows object-oriented design and the
dependency direction `Api -> Infrastructure -> Application -> Domain`.
Use cases, entities, repository contracts, SQL repositories, controllers, and
static presentation are separated into cohesive modules.

Evidence: `HEJCareSuite.sln`, `docs/architecture-and-decisions.md`, `src/`.

## Stage 7 - Database project

**Implemented baseline.** The physical model contains `Patient`, `UserAccount`,
`Triage`, `HospitalAdmission`, `AuditLog`, and `SchemaVersion`, with primary
keys, foreign keys, constraints, indexes, incremental scripts, and audit
triggers. Script `006_validation_queries.sql` is read-only and provides
evidence for the report.

Stored procedures are not required by the current API because repositories use
parameterized ADO.NET commands. The report must describe this decision instead
of claiming procedures exist.

Evidence: `database/001_initial_schema.sql` through
`database/006_validation_queries.sql`.

## Stage 8 - Cloud and DevOps

**Implemented baseline and documented plan.** The repository contains a
multi-stage Dockerfile, GitHub Actions CI, environment-based connection
configuration, a container health smoke test, and deployment documentation.
The plan covers cloud hosting, managed SQL Server, secrets, monitoring,
scalability, and production approvals.

The current validated deployment target is Codespaces/container execution; a
production cloud provider has not been provisioned in this repository.

Evidence: `Dockerfile`, `.github/workflows/ci.yml`,
`docs/deployment-guide.md`, `docs/final-validation-runbook.md`.

## ABNT writing guidance

Use the status words **implemented**, **implemented baseline**, and **planned**
exactly as above. Include screenshots of the responsive PWA, VLibras control,
login, patient workflow, triage, admission/discharge, SSMS audit output, CI,
and container health response. Do not include passwords, hashes, salts,
connection strings, or fabricated cloud metrics.
