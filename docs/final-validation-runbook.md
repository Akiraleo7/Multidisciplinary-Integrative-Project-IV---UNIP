/*
    HEJ Care Suite - Final Validation Runbook (Updated for Mobile & Accessibility)
    This version documents the completion of the mobile MVP and VLibras integration.
*/

## 1. Update and verify source
git fetch origin
git switch feature-final-integrations
git pull --ff-only origin feature-final-integrations

## 2. Validate application and frontend
dotnet clean
dotnet build
dotnet test
node --check src/HEJCareSuite.Api/wwwroot/app.js
node --check src/HEJCareSuite.Api/wwwroot/vlibras-init.js

## 3. Validate database
Apply `database/007_mobile_and_accessibility_schema.sql` in SSMS, then run `database/006_validation_queries.sql` (updated manually for new tables).

## 4. Run the application
1. Start API (dotnet run).
2. `GET /health` returns HTTP 200.
3. Login as `admin@hejcare.local`.
4. Verify VLibras widget initializes in bottom-left corner.
5. Mobile endpoints:
   - `POST /api/v1/mobile/tasks`
   - `GET /api/v1/mobile/tasks/patient/{id}/pending`
   - `POST /api/v1/mobile/alerts`
   - `GET /api/v1/mobile/alerts/unacknowledged/{id?}`
6. Logout blocks protected operations.
