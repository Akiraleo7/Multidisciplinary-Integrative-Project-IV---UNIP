/*
    HEJ Care Suite - Initial database schema
    Requirement coverage: FR-01, NFR-02, NFR-05

    This script is intentionally idempotent so it can be safely executed from SSMS
    while the database is being initialized in the Codespace.
*/

IF DB_ID(N'HEJCareSuite') IS NULL
BEGIN
    CREATE DATABASE [HEJCareSuite];
END;
GO

USE [HEJCareSuite];
GO

IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchemaVersion
    (
        VersionNumber INT NOT NULL,
        AppliedAtUtc DATETIME2(3) NOT NULL
            CONSTRAINT DF_SchemaVersion_AppliedAtUtc DEFAULT SYSUTCDATETIME(),
        CONSTRAINT PK_SchemaVersion PRIMARY KEY (VersionNumber)
    );
END;
GO

IF OBJECT_ID(N'dbo.Patient', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Patient
    (
        PatientId UNIQUEIDENTIFIER NOT NULL
            CONSTRAINT DF_Patient_PatientId DEFAULT NEWSEQUENTIALID(),
        FullName NVARCHAR(200) NOT NULL,
        BirthDate DATE NOT NULL,
        DocumentNumber NVARCHAR(40) NOT NULL,
        CreatedAtUtc DATETIME2(3) NOT NULL
            CONSTRAINT DF_Patient_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
        UpdatedAtUtc DATETIME2(3) NULL,
        CONSTRAINT PK_Patient PRIMARY KEY (PatientId),
        CONSTRAINT UQ_Patient_DocumentNumber UNIQUE (DocumentNumber),
        CONSTRAINT CK_Patient_FullName_NotBlank CHECK (LEN(LTRIM(RTRIM(FullName))) > 0),
        CONSTRAINT CK_Patient_DocumentNumber_NotBlank
            CHECK (LEN(LTRIM(RTRIM(DocumentNumber))) > 0),
        CONSTRAINT CK_Patient_BirthDate_Valid CHECK (BirthDate >= '1900-01-01')
    );
END;
GO

IF OBJECT_ID(N'dbo.AuditLog', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AuditLog
    (
        AuditLogId BIGINT IDENTITY(1, 1) NOT NULL,
        EntityName NVARCHAR(128) NOT NULL,
        EntityId UNIQUEIDENTIFIER NULL,
        ActionName NVARCHAR(20) NOT NULL,
        OccurredAtUtc DATETIME2(3) NOT NULL
            CONSTRAINT DF_AuditLog_OccurredAtUtc DEFAULT SYSUTCDATETIME(),
        Details NVARCHAR(MAX) NULL,
        CONSTRAINT PK_AuditLog PRIMARY KEY (AuditLogId),
        CONSTRAINT CK_AuditLog_ActionName
            CHECK (ActionName IN ('INSERT', 'UPDATE', 'DELETE'))
    );
END;
GO

IF OBJECT_ID(N'dbo.TR_Patient_AuditLog', N'TR') IS NULL
BEGIN
    EXEC(N'
        CREATE TRIGGER dbo.TR_Patient_AuditLog
        ON dbo.Patient
        AFTER INSERT, UPDATE, DELETE
        AS
        BEGIN
            SET NOCOUNT ON;

            INSERT INTO dbo.AuditLog (EntityName, EntityId, ActionName, Details)
            SELECT
                N''Patient'',
                COALESCE(inserted.PatientId, deleted.PatientId),
                CASE
                    WHEN inserted.PatientId IS NOT NULL AND deleted.PatientId IS NULL
                        THEN N''INSERT''
                    WHEN inserted.PatientId IS NOT NULL AND deleted.PatientId IS NOT NULL
                        THEN N''UPDATE''
                    ELSE N''DELETE''
                END,
                N''Patient record changed''
            FROM inserted
            FULL OUTER JOIN deleted
                ON inserted.PatientId = deleted.PatientId;
        END;
    ');
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.SchemaVersion
    WHERE VersionNumber = 1
)
BEGIN
    INSERT INTO dbo.SchemaVersion (VersionNumber)
    VALUES (1);
END;
GO
