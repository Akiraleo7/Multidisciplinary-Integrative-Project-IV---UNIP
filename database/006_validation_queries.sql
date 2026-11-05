/*
    HEJ Care Suite - Database validation queries

    This script is read-only. Execute it in SSMS after applying schemas 001-005
    to collect evidence for the technical and academic documentation.
*/

USE [HEJCareSuite];
GO

-- 1. Applied schema versions
SELECT
    VersionNumber,
    AppliedAtUtc
FROM dbo.SchemaVersion
ORDER BY VersionNumber;
GO

-- 2. Expected application tables
SELECT
    s.name AS SchemaName,
    t.name AS TableName,
    t.create_date AS CreatedAt
FROM sys.tables AS t
INNER JOIN sys.schemas AS s ON s.schema_id = t.schema_id
WHERE s.name = N'dbo'
  AND t.name IN
  (
      N'Patient',
      N'UserAccount',
      N'Triage',
      N'HospitalAdmission',
      N'AuditLog',
      N'SchemaVersion'
  )
ORDER BY t.name;
GO

-- 3. Foreign keys and check/unique constraints
SELECT
    OBJECT_SCHEMA_NAME(parent_object_id) AS SchemaName,
    OBJECT_NAME(parent_object_id) AS TableName,
    name AS ConstraintName,
    type_desc AS ConstraintType
FROM sys.objects
WHERE parent_object_id IN
(
    OBJECT_ID(N'dbo.Patient'),
    OBJECT_ID(N'dbo.UserAccount'),
    OBJECT_ID(N'dbo.Triage'),
    OBJECT_ID(N'dbo.HospitalAdmission'),
    OBJECT_ID(N'dbo.AuditLog'),
    OBJECT_ID(N'dbo.SchemaVersion')
)
AND type IN (N'F', N'UQ', N'C', N'PK')
ORDER BY TableName, ConstraintType, ConstraintName;
GO

-- 4. Operational and filtered indexes
SELECT
    OBJECT_SCHEMA_NAME(i.object_id) AS SchemaName,
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    i.is_unique AS IsUnique,
    i.has_filter AS HasFilter,
    i.filter_definition AS FilterDefinition
FROM sys.indexes AS i
WHERE i.object_id IN
(
    OBJECT_ID(N'dbo.Patient'),
    OBJECT_ID(N'dbo.UserAccount'),
    OBJECT_ID(N'dbo.Triage'),
    OBJECT_ID(N'dbo.HospitalAdmission'),
    OBJECT_ID(N'dbo.AuditLog')
)
AND i.index_id > 0
ORDER BY TableName, IndexName;
GO

-- 5. Audit triggers
SELECT
    OBJECT_SCHEMA_NAME(parent_id) AS SchemaName,
    OBJECT_NAME(parent_id) AS TableName,
    name AS TriggerName,
    is_disabled AS IsDisabled
FROM sys.triggers
WHERE parent_id IN
(
    OBJECT_ID(N'dbo.Patient'),
    OBJECT_ID(N'dbo.Triage'),
    OBJECT_ID(N'dbo.HospitalAdmission')
)
ORDER BY TableName, TriggerName;
GO

-- 6. Row counts for the MVP tables
SELECT N'Patient' AS TableName, COUNT_BIG(*) AS [RowCount] FROM dbo.Patient
UNION ALL
SELECT N'UserAccount', COUNT_BIG(*) FROM dbo.UserAccount
UNION ALL
SELECT N'Triage', COUNT_BIG(*) FROM dbo.Triage
UNION ALL
SELECT N'HospitalAdmission', COUNT_BIG(*) FROM dbo.HospitalAdmission
UNION ALL
SELECT N'AuditLog', COUNT_BIG(*) FROM dbo.AuditLog;
GO

-- 7. Recent audit evidence
SELECT TOP (50)
    AuditLogId,
    EntityName,
    EntityId,
    ActionName,
    OccurredAtUtc,
    Details
FROM dbo.AuditLog
ORDER BY OccurredAtUtc DESC, AuditLogId DESC;
GO
