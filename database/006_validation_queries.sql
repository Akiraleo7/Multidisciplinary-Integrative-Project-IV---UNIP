/*
    HEJ Care Suite - Database validation queries (Updated for Mobile & Accessibility)
    This script is read-only. Execute it in SSMS after applying schemas 001-007
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
      N'StaffTask',
      N'CriticalAlert',
      N'MobileSyncLog',
      N'AuditLog',
      N'SchemaVersion'
  )
ORDER BY t.name;
GO

-- 3. Row counts for all MVP & Mobile tables
SELECT N'Patient' AS TableName, COUNT_BIG(*) AS [RowCount] FROM dbo.Patient
UNION ALL
SELECT N'UserAccount', COUNT_BIG(*) FROM dbo.UserAccount
UNION ALL
SELECT N'Triage', COUNT_BIG(*) FROM dbo.Triage
UNION ALL
SELECT N'HospitalAdmission', COUNT_BIG(*) FROM dbo.HospitalAdmission
UNION ALL
SELECT N'StaffTask', COUNT_BIG(*) FROM dbo.StaffTask
UNION ALL
SELECT N'CriticalAlert', COUNT_BIG(*) FROM dbo.CriticalAlert
UNION ALL
SELECT N'AuditLog', COUNT_BIG(*) FROM dbo.AuditLog;
GO

-- 4. Recent audit evidence
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
