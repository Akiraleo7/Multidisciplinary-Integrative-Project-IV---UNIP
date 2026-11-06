# HEJ Care Suite - Deployment Guide

## Prerequisites

- Docker Engine or a compatible container runtime.
- A reachable SQL Server database initialized with scripts `001` through `005`.
- A deployment-specific SQL Server connection string.

## Build the application image

From the repository root:

```bash
docker build --tag hejcare-suite:latest .
```

## Run the application

Provide the connection string through the environment. Do not commit credentials
or place them in the image.

```bash
docker run --detach \
  --name hejcare-suite \
  --publish 8080:8080 \
  --env "ConnectionStrings__SqlServer=Server=sqlserver,1433;Database=HEJCareSuite;User Id=sa;Password=<secret>;TrustServerCertificate=True;Encrypt=False" \
  hejcare-suite:latest
```

The application listens on port `8080`.

## Smoke test

Verify that the process is running:

```bash
curl --fail http://localhost:8080/health
```

Expected response:

```json
{"status":"ok","service":"HEJCareSuite.Api"}
```

Then open `http://localhost:8080` and validate login, patient management,
triage, admission, and discharge using a database that has the required schema.

## Database deployment order

For a new database, execute:

```text
001_initial_schema.sql
002_operational_indexes.sql
003_authentication_schema.sql
004_triage_schema.sql
005_hospitalization_schema.sql
```

For the existing project database, do not delete the database. Apply only the
pending or updated scripts according to the current `dbo.SchemaVersion` values,
then execute `006_validation_queries.sql` to collect evidence.
