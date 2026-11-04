IF DB_ID(N'HEJCareSuite') IS NULL
BEGIN
    THROW 50000, 'Database HEJCareSuite must exist before applying this script.', 1;
END;
GO

USE HEJCareSuite;
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'HospitalAdmission')
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
    SELECT 1 FROM dbo.SchemaVersion WHERE VersionNumber = 5
)
BEGIN
    INSERT INTO dbo.SchemaVersion (VersionNumber)
    VALUES (5);
END;
GO
