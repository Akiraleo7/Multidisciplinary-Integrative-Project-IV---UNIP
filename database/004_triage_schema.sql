/*
    HEJ Care Suite - Emergency triage
    Requirement coverage: FR-02, BR-01, NFR-02
*/

USE [HEJCareSuite];
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

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Triage_Patient')
BEGIN
    CREATE INDEX IX_Triage_Patient ON dbo.Triage (PatientId, RecordedAtUtc DESC);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE VersionNumber = 4)
BEGIN
    INSERT INTO dbo.SchemaVersion (VersionNumber) VALUES (4);
END;
GO
