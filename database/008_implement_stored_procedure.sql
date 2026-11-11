/*
    HEJ Care Suite - Stored Procedures for Optimized Clinical Data Access
    Requirement coverage: Etapa 7 (Database Project - Stored Procedures)
*/

IF DB_ID(N'HEJCareSuite') IS NULL
BEGIN
    THROW 50050, 'Database HEJCareSuite must exist before applying this script.', 1;
END;
GO

USE [HEJCareSuite];
GO

IF OBJECT_ID(N'dbo.PR_GetPatientClinicalSummary', N'P') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.PR_GetPatientClinicalSummary;
END;
GO

/*
    PR_GetPatientClinicalSummary
    Provides a unified view of a patient's clinical state, including the last triage,
    active admission, and pending tasks. Used by the Mobile Clinical App.
*/
CREATE PROCEDURE dbo.PR_GetPatientClinicalSummary
    @PatientId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Basic Patient Info
    SELECT
        PatientId, FullName, BirthDate, DocumentNumber, CreatedAtUtc
    FROM dbo.Patient
    WHERE PatientId = @PatientId;

    -- 2. Latest Triage
    SELECT TOP (1)
        TriageId, RiskLevel, ChiefComplaint, RecordedAtUtc
    FROM dbo.Triage
    WHERE PatientId = @PatientId
    ORDER BY RecordedAtUtc DESC;

    -- 3. Current Active Admission
    SELECT TOP (1)
        AdmissionId, BedNumber, AdmittedAtUtc, Status
    FROM dbo.HospitalAdmission
    WHERE PatientId = @PatientId AND Status = 1 -- Active
    ORDER BY AdmittedAtUtc DESC;

    -- 4. Recent Staff Tasks (limit 10)
    SELECT TOP (10)
        TaskId, Title, Priority, IsCompleted, AssignedRole, DueDateUtc
    FROM dbo.StaffTask
    WHERE PatientId = @PatientId
    ORDER BY IsCompleted ASC, Priority DESC, CreatedAtUtc DESC;

END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE VersionNumber = 8)
BEGIN
    INSERT INTO dbo.SchemaVersion (VersionNumber) VALUES (8);
END;
GO
