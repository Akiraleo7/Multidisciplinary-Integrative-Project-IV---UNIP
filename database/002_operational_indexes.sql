/*
    HEJ Care Suite - Operational indexes
    Requirement coverage: FR-01, NFR-02

    Run after 001_initial_schema.sql. The statements are idempotent.
*/

IF DB_ID(N'HEJCareSuite') IS NULL
BEGIN
    THROW 50009, 'Database HEJCareSuite must exist before applying this script.', 1;
END;
GO

USE [HEJCareSuite];
GO

IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
BEGIN
    THROW 50012, 'Table dbo.SchemaVersion must exist before applying this script.', 1;
END;
GO

IF OBJECT_ID(N'dbo.Patient', N'U') IS NULL
BEGIN
    THROW 50010, 'Table dbo.Patient must exist before applying this script.', 1;
END;
GO

IF OBJECT_ID(N'dbo.AuditLog', N'U') IS NULL
BEGIN
    THROW 50011, 'Table dbo.AuditLog must exist before applying this script.', 1;
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Patient_FullName'
      AND object_id = OBJECT_ID(N'dbo.Patient')
)
BEGIN
    CREATE INDEX IX_Patient_FullName
        ON dbo.Patient (FullName);
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_AuditLog_Entity'
      AND object_id = OBJECT_ID(N'dbo.AuditLog')
)
BEGIN
    CREATE INDEX IX_AuditLog_Entity
        ON dbo.AuditLog (EntityName, EntityId, OccurredAtUtc DESC);
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.SchemaVersion
    WHERE VersionNumber = 2
)
BEGIN
    INSERT INTO dbo.SchemaVersion (VersionNumber)
    VALUES (2);
END;
GO
