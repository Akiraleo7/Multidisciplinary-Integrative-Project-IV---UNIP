# HEJ Care Suite - MVP API Reference

Base URL: `http://localhost:8080`

The API uses cookie authentication. Call `POST /api/v1/auth/login` first and
reuse the returned HTTP cookie for protected endpoints.

## Health and authentication

The health endpoint is a technical availability probe, not a clinical
dashboard. It stays unauthenticated so a container runtime, load balancer,
operator, or CI job can confirm that the API process is reachable before
testing protected workflows.

| Method | Route | Authentication | Success |
| --- | --- | --- | --- |
| GET | `/health` | None | `200 OK` with service status |
| POST | `/api/v1/auth/login` | None | `200 OK` and authentication cookie |
| POST | `/api/v1/auth/logout` | Authenticated session | `204 No Content` |

Login request:

```json
{
  "email": "admin@hejcare.local",
  "password": "ChangeMe123!"
}
```

The demonstration password is for local validation only and must be replaced
before shared or production use.

## Patients

All patient routes require authentication.

| Method | Route | Success |
| --- | --- | --- |
| GET | `/api/v1/patients` | `200 OK` with patient collection |
| POST | `/api/v1/patients` | `201 Created` |
| PUT | `/api/v1/patients/{patientId}` | `204 No Content` |
| DELETE | `/api/v1/patients/{patientId}` | `204 No Content` |

Patient create/update request:

```json
{
  "fullName": "Maria Silva",
  "birthDate": "1990-05-20",
  "documentNumber": "12345678900"
}
```

## Emergency triage

All triage routes require authentication.

| Method | Route | Success |
| --- | --- | --- |
| GET | `/api/v1/patients/{patientId}/triage` | `200 OK` with triage history |
| POST | `/api/v1/patients/{patientId}/triage` | `201 Created` |

Triage request:

```json
{
  "riskLevel": 2,
  "chiefComplaint": "Persistent chest discomfort"
}
```

Risk levels are represented by values `1` through `4` in the current MVP.

## Hospitalization

All hospitalization routes require authentication.

| Method | Route | Success |
| --- | --- | --- |
| GET | `/api/v1/patients/{patientId}/admissions` | `200 OK` with admission history |
| POST | `/api/v1/patients/{patientId}/admissions` | `201 Created` |
| POST | `/api/v1/patients/{patientId}/admissions/{admissionId}/discharge` | `204 No Content` |

Admission request:

```json
{
  "bedNumber": "A-101"
}
```

Discharge request:

```json
{
  "dischargeNotes": "Patient discharged with follow-up instructions."
}
```

## Error behavior

- `401 Unauthorized`: the request requires an authenticated cookie session.
- `404 Not Found`: the requested patient or admission does not exist.
- `400 Bad Request`: request validation or a domain rule failed.
- Validation failures use an RFC 7807-style ProblemDetails response when handled
  by the API exception pipeline.

The API reference covers the presentation MVP. Exams, pharmacy, dashboards, and
native mobile endpoints are not implemented in this release.
