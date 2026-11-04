/*
    HEJ Care Suite - Emergency triage
    Requirement coverage: FR-02, BR-01, NFR-02
*/

IF DB_ID(N'HEJCareSuite') IS NULL
BEGIN
    THROW 50029, 'Database HEJCareSuite must exist before applying this script.', 1;
END;
GO

USE [HEJCareSuite];
GO

IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
BEGIN
    THROW 50032, 'Table dbo.SchemaVersion must exist before applying this script.', 1;
END;
GO

IF OBJECT_ID(N'dbo.Patient', N'U') IS NULL
BEGIN
    THROW 50030, 'Table dbo.Patient must exist before applying this script.', 1;
END;
GO

IF OBJECT_ID(N'dbo.AuditLog', N'U') IS NULL
BEGIN
    THROW 50031, 'Table dbo.AuditLog must exist before applying this script.', 1;
END;
GO

IF OBJECT_ID(N'dbo.Triage', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Triage
    (
        TriageId UNIQUEIDENTIFIER NOT NULL
            CONSTRAINT DF_Triage_TriageId DEFAULT NEWSEQUENTIALID(),
        PatientId UNIQUEIDENTIFIER NOT NULL,
        RiskLevel INT NOT NULL,
        ChiefComplaint NVARCHAR(500) NOT NULL,
        RecordedAtUtc DATETIME2(3) NOT NULL
            CONSTRAINT DF_Triage_RecordedAtUtc DEFAULT SYSUTCDATETIME(),
        CONSTRAINT PK_Triage PRIMARY KEY (TriageId),
        CONSTRAINT FK_Triage_Patient FOREIGN KEY (PatientId) REFERENCES dbo.Patient(PatientId),
        CONSTRAINT CK_Triage_RiskLevel CHECK (RiskLevel BETWEEN 1 AND 4),
        CONSTRAINT CK_Triage_Complaint_NotBlank CHECK (LEN(LTRIM(RTRIM(ChiefComplaint))) > 0)
    );
END;
GO

IF OBJECT_ID(N'dbo.TR_Triage_AuditLog', N'TR') IS NULL
BEGIN
    EXEC(N'
        CREATE TRIGGER dbo.TR_Triage_AuditLog
        ON dbo.Triage
        AFTER INSERT, UPDATE, DELETE
        AS
        BEGIN
            SET NOCOUNT ON;

            INSERT INTO dbo.AuditLog (EntityName, EntityId, ActionName, Details)
            SELECT
                N''Triage'',
                COALESCE(inserted.TriageId, deleted.TriageId),
                CASE
                    WHEN inserted.TriageId IS NOT NULL AND deleted.TriageId IS NULL
                        THEN N''INSERT''
                    WHEN inserted.TriageId IS NOT NULL AND deleted.TriageId IS NOT NULL
                        THEN N''UPDATE''
                    ELSE N''DELETE''
                END,
                N''Triage record changed''
            FROM inserted
            FULL OUTER JOIN deleted
                ON inserted.TriageId = deleted.TriageId;
        END;
    ');
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Triage_Patient'
      AND object_id = OBJECT_ID(N'dbo.Triage')
)
BEGIN
    CREATE INDEX IX_Triage_Patient ON dbo.Triage (PatientId, RecordedAtUtc DESC);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE VersionNumber = 4)
BEGIN
    INSERT INTO dbo.SchemaVersion (VersionNumber) VALUES (4);
END;
GO
