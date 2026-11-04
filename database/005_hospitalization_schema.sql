/*
    HEJ Care Suite - Hospital admission and discharge
    Requirement coverage: FR-03, NFR-02, NFR-05

    This script is idempotent and must be applied after 001_initial_schema.sql.
*/

IF DB_ID(N'HEJCareSuite') IS NULL
BEGIN
    THROW 50000, 'Database HEJCareSuite must exist before applying this script.', 1;
END;
GO

USE [HEJCareSuite];
GO

IF OBJECT_ID(N'dbo.Patient', N'U') IS NULL
BEGIN
    THROW 50001, 'Table dbo.Patient must exist before applying this script.', 1;
END;
GO

IF OBJECT_ID(N'dbo.AuditLog', N'U') IS NULL
BEGIN
    THROW 50002, 'Table dbo.AuditLog must exist before applying this script.', 1;
END;
GO

IF OBJECT_ID(N'dbo.HospitalAdmission', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HospitalAdmission
    (
        AdmissionId UNIQUEIDENTIFIER NOT NULL
            CONSTRAINT PK_HospitalAdmission PRIMARY KEY,
        PatientId UNIQUEIDENTIFIER NOT NULL,
        BedNumber NVARCHAR(40) NOT NULL,
        Status INT NOT NULL
            CONSTRAINT CK_HospitalAdmission_Status CHECK (Status IN (1, 2)),
        AdmittedAtUtc DATETIME2 NOT NULL,
        DischargedAtUtc DATETIME2 NULL,
        DischargeNotes NVARCHAR(2000) NULL,
        CONSTRAINT CK_HospitalAdmission_Bed_NotBlank
            CHECK (LEN(LTRIM(RTRIM(BedNumber))) > 0),
        CONSTRAINT FK_HospitalAdmission_Patient
            FOREIGN KEY (PatientId) REFERENCES dbo.Patient(PatientId),
        CONSTRAINT CK_HospitalAdmission_DischargeData CHECK
        (
            (Status = 1 AND DischargedAtUtc IS NULL AND DischargeNotes IS NULL)
            OR
            (Status = 2 AND DischargedAtUtc IS NOT NULL AND DischargeNotes IS NOT NULL
                AND LEN(LTRIM(RTRIM(DischargeNotes))) > 0)
        )
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.check_constraints
    WHERE name = N'CK_HospitalAdmission_Bed_NotBlank'
      AND parent_object_id = OBJECT_ID(N'dbo.HospitalAdmission')
)
BEGIN
    ALTER TABLE dbo.HospitalAdmission
        ADD CONSTRAINT CK_HospitalAdmission_Bed_NotBlank
            CHECK (LEN(LTRIM(RTRIM(BedNumber))) > 0);
END;
GO

IF OBJECT_ID(N'dbo.TR_HospitalAdmission_AuditLog', N'TR') IS NULL
BEGIN
    EXEC(N'
        CREATE TRIGGER dbo.TR_HospitalAdmission_AuditLog
        ON dbo.HospitalAdmission
        AFTER INSERT, UPDATE, DELETE
        AS
        BEGIN
            SET NOCOUNT ON;

            INSERT INTO dbo.AuditLog (EntityName, EntityId, ActionName, Details)
            SELECT
                N''HospitalAdmission'',
                COALESCE(inserted.AdmissionId, deleted.AdmissionId),
                CASE
                    WHEN inserted.AdmissionId IS NOT NULL AND deleted.AdmissionId IS NULL
                        THEN N''INSERT''
                    WHEN inserted.AdmissionId IS NOT NULL AND deleted.AdmissionId IS NOT NULL
                        THEN N''UPDATE''
                    ELSE N''DELETE''
                END,
                N''Hospital admission record changed''
            FROM inserted
            FULL OUTER JOIN deleted
                ON inserted.AdmissionId = deleted.AdmissionId;
        END;
    ');
END;
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE name = N'IX_HospitalAdmission_Patient_AdmittedAtUtc'
      AND object_id = OBJECT_ID(N'dbo.HospitalAdmission')
)
BEGIN
    CREATE INDEX IX_HospitalAdmission_Patient_AdmittedAtUtc
        ON dbo.HospitalAdmission (PatientId, AdmittedAtUtc DESC);
END;
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE name = N'UX_HospitalAdmission_ActiveBed'
      AND object_id = OBJECT_ID(N'dbo.HospitalAdmission')
)
BEGIN
    CREATE UNIQUE INDEX UX_HospitalAdmission_ActiveBed
        ON dbo.HospitalAdmission (BedNumber)
        WHERE Status = 1;
END;
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE name = N'UX_HospitalAdmission_ActivePatient'
      AND object_id = OBJECT_ID(N'dbo.HospitalAdmission')
)
BEGIN
    CREATE UNIQUE INDEX UX_HospitalAdmission_ActivePatient
        ON dbo.HospitalAdmission (PatientId)
        WHERE Status = 1;
END;
GO

IF NOT EXISTS
(
    SELECT 1 FROM dbo.SchemaVersion WHERE VersionNumber = 5
)
BEGIN
    INSERT INTO dbo.SchemaVersion (VersionNumber)
    VALUES (5);
END;
GO
