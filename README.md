# Hospital Edward Jenner Digital Transformation Platform (PIM IV)

## 1) Project Context

This repository contains the integrated plan and implementation baseline for **PIM IV** (UNIP – ADS), based on a fictional healthcare company:

- **Company:** Hospital Edward Jenner  
- **Industry:** Healthcare  
- **Target Audience:** People of all ages, classes, and profiles needing medical assistance  
- **Core Services:** Emergency care, hospitalization, outpatient care, 24/7 nursing, diagnostics (lab + radiology), hospital pharmacy, sterilization center (CME), nutrition, and healthcare waste management.

The main business need is to improve operational management, process organization, and patient experience through a complete digital solution.

---

## 2) General Objective (PIM IV)

Design and implement a complete and integrated technological solution covering:

- Business planning and viability
- Web and mobile development
- API integration
- Object-oriented modular architecture (MVC + layered design)
- Optimized corporate database
- Cloud infrastructure and DevOps (CI/CD)
- Agile project management (Scrum)
- Inclusion, diversity, accessibility, and social responsibility

---

## 3) Proposed Solution Overview

### Product Name
**HEJ Care Suite** (Hospital Edward Jenner Care Suite)

### Problem to Solve
Hospital workflows are fragmented, slow, and hard to monitor across triage, care, diagnostics, pharmacy, and support services.

### Solution Goals
1. Centralize patient and care data.
2. Improve emergency and hospitalization workflows.
3. Reduce operational bottlenecks and manual errors.
4. Provide integrated web + mobile experience.
5. Enable secure and scalable digital operations.

### Value Proposition
A modular hospital platform that unifies clinical and administrative processes with secure, real-time, and accessible services.

### Expected Benefits
- Faster patient intake and triage
- Better bed/room visibility
- Better traceability of exams, medication, and procedures
- Reduced communication noise between teams
- Better management indicators for decisions

### Competitive Differentials
- End-to-end integration (Web + API + Mobile)
- Accessibility-first and inclusive design
- SCRUM delivery model with measurable increments
- Cloud-ready architecture with CI/CD

---

## 4) Scope (MVP for PIM IV)

### In Scope
- Authentication and role-based authorization
- Patient registration and records
- Emergency triage and care queue
- Hospital admission/discharge workflow
- Exam request and results visualization
- Medication dispensing tracking
- Basic dashboards and operational reports
- Mobile app for staff task tracking and alerts

### Out of Scope (current semester MVP)
- Full financial billing engine
- Advanced telemedicine/video consultation
- AI-assisted diagnosis in production

---

## 5) Requirements Engineering Baseline

### Functional Requirements (sample)
- **FR-01:** Register and manage patients.
- **FR-02:** Register emergency triage and risk classification.
- **FR-03:** Manage hospitalization admission, bed allocation, and discharge.
- **FR-04:** Request and track lab/radiology exams.
- **FR-05:** Control medication dispensing through pharmacy workflow.
- **FR-06:** Provide role-based dashboards for operations.
- **FR-07:** Expose REST APIs for web/mobile integration.

### Non-Functional Requirements (sample)
- **NFR-01:** Authentication with secure password hashing and session/token control.
- **NFR-02:** Audit trail for critical operations.
- **NFR-03:** Responsive web interface and mobile usability.
- **NFR-04:** Availability suitable for hospital routine operations.
- **NFR-05:** Data protection aligned with healthcare privacy principles.

### Business Rules (sample)
- **BR-01:** Emergency triage must be recorded before non-critical outpatient processing.
- **BR-02:** Medication cannot be dispensed without a valid prescription record.
- **BR-03:** Discharge requires final physician confirmation.

### Traceability
Every implemented feature must be mapped to:
1. Requirement ID (FR/NFR/BR)
2. Jira issue key
3. Sprint and acceptance criteria

---

## 6) Architecture and Engineering Standards

### Architectural Style
- **MVC** for presentation layer
- **Layered architecture** for separation of concerns:
  - Presentation (Web/Mobile)
  - Application (use cases/services)
  - Domain (entities/rules)
  - Infrastructure (DB/external integrations)

### Design and Code Quality Principles
- Object-oriented design with SOLID
- Clear modular boundaries
- Clean code naming and small cohesive classes
- Exception handling with user-friendly errors and structured logs

### API Strategy
- RESTful endpoints
- Versioned routes (e.g., `/api/v1/...`)
- Standardized response contracts
- Authorization by role/profile

---

## 7) Database Plan (PIM IV + PIM III continuity goal)

### Data Modeling Deliverables
- Conceptual model (entities and relationships)
- Logical model (normalized up to 3NF when applicable)
- Physical model (tables, constraints, indexes)
- MER/ERD diagrams
- Stored procedures and triggers where justified
- Full SQL script used by API, Web, and Mobile modules

### Core Entities (initial)
- Patient
- Professional (doctor/nurse/technician)
- Triage
- Consultation
- Admission
- Bed
- ExamRequest
- ExamResult
- Prescription
- Dispensation
- AuditLog

---

## 8) Web + Mobile Deliverables

### Web Application
- Administrative and operational modules
- Secure authentication and authorization
- Responsive UI and accessible interactions
- Integration with all core APIs

### Mobile Application
- Authentication
- Task/status views for care teams
- Critical alerts/notifications
- Data synchronization with backend APIs

---

## 9) Cloud, DevOps, and Deployment Plan

### Cloud Infrastructure (high level)
- Application hosting for API/Web
- Managed relational database
- Object storage for documents/results (if required)
- Monitoring and observability

### DevOps Baseline
- Containerization for services
- CI/CD pipeline for build, test, and deploy automation
- Environment separation: Dev / Homolog / Prod
- Secret and configuration management by environment

### Deployment Flow (from scratch to production)
1. Define environments and variables.
2. Configure CI (build + tests + quality checks).
3. Configure CD pipeline with approval gates.
4. Run DB migrations in target environment.
5. Deploy API, Web, and Mobile release artifacts.
6. Validate smoke tests in production.

---

## 10) Agile Management (SCRUM) with Jira + Confluence

### Scrum Structure
- Roles: Product Owner, Scrum Master, Development Team
- Ceremonies: Planning, Daily, Review, Retrospective
- Artifacts: Product Backlog, Sprint Backlog, Increment, Definition of Done

### Atlassian Usage
- **Jira:** Backlog, sprint planning, Kanban/Scrum board, issue workflow, burndown
- **Confluence:** Technical documentation, architecture decisions, meeting notes, test evidence, deployment guides

### Suggested Jira Workflow
`Backlog -> Selected for Sprint -> In Progress -> Code Review -> Testing -> Done`

---

## 11) Inclusion, Diversity, Accessibility, and Social Responsibility

The platform must explicitly include:

- Anti-discrimination and respectful language standards
- Accessibility resources (keyboard navigation, contrast, readable labels, semantic components)
- Inclusive communication for broad patient profiles
- Features/processes that improve social access to healthcare services

---

## 12) Step-by-Step Roadmap (Start to Deploy)

1. Define business scope and stakeholders.
2. Elicit and approve FR/NFR/BR requirements.
3. Build use cases and traceability matrix.
4. Model database (conceptual/logical/physical + scripts).
5. Define MVC + layered architecture and module boundaries.
6. Design wireframes and navigation flows.
7. Configure repository standards, branching, and CI.
8. Implement APIs and core web modules sprint by sprint.
9. Implement mobile integration features.
10. Add tests (unit + integration) and quality gates.
11. Configure cloud infrastructure and deployment pipeline.
12. Execute homologation, production deploy, and post-deploy validation.

---

## 13) Academic Delivery Alignment (PIM IV Stages)

This repository baseline aligns with the nine PIM IV stages:

1. Organization characterization  
2. Technology solution planning  
3. Social responsibility and diversity  
4. Web solution development  
5. Mobile solution development  
6. Software architecture  
7. Database project  
8. Cloud + DevOps plan  
9. Agile project management  

---

## 14) Team Collaboration Notes

- All technical and management documentation must remain in **English**.
- Project management and engineering workflow must use **SCRUM**.
- Architecture and implementation must follow **MVC** and layered design.
- Team coordination (6 members) should be managed through **Jira** and **Confluence**.

---

## 15) Local Development with GitHub Codespaces

The repository includes a Dev Container with two services:

- `app`: .NET 8 SDK used to build and run the solution.
- `sqlserver`: SQL Server 2022 Developer Edition exposed on port `1433`.

Before creating a Codespace, add `MSSQL_SA_PASSWORD` as a Codespaces repository secret. The
password must meet SQL Server complexity requirements and must not be committed to the repository.
SSMS can connect to the forwarded port using:

```text
Server: localhost,1433
Authentication: SQL Server Authentication
Login: sa
Password: the value of MSSQL_SA_PASSWORD
```

The initial backend baseline is under `src/` and follows the dependency direction
`Api -> Infrastructure -> Application -> Domain`. Patient data is persisted in SQL Server
through the connection string supplied to the app container by Docker Compose.

The first SQL Server schema is available at `database/001_initial_schema.sql`. Execute it in
SSMS after connecting to the Codespace SQL Server. It creates the `HEJCareSuite` database,
`Patient`, `AuditLog`, and `SchemaVersion` tables, plus the patient audit trigger.
