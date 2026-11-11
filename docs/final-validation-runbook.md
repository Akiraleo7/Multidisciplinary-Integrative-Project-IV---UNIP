/*
    HEJ Care Suite - Final Validation Runbook (Updated for Mobile & Accessibility)
    This version documents the completion of the full PIM IV scope.
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
1. Apply `database/007_mobile_and_accessibility_schema.sql`.
2. Apply `database/008_implement_stored_procedure.sql`.
3. Run `database/006_validation_queries.sql` and verify Schema Version is 8.
4. Execute Procedure: `EXEC dbo.PR_GetPatientClinicalSummary @PatientId = '...'`.

## 4. Run the application
1. Start API (`dotnet run` inside `src/HEJCareSuite.Api`).
2. `GET /health` returns HTTP 200.
3. Login as `admin@hejcare.local`.
4. **Web Desktop:** Verify VLibras widget initializes.
5. **Mobile Prototype:** Access `http://localhost:8080/mobile/index.html`.
   - Verify responsive layout.
   - Test "Sincronizar Agora" button (calls health API).
   - Verify VLibras widget presence.
6. **Mobile APIs:**
   - `POST /api/v1/mobile/tasks`
   - `GET /api/v1/mobile/tasks/patient/{id}/pending`
   - `POST /api/v1/mobile/alerts`
7. Logout blocks protected operations.
