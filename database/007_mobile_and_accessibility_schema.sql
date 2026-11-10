/*
    HEJ Care Suite - Mobile support & VLIBRAS accessibility schema
    Requirement coverage: FR-07 (Mobile APIs), NFR-03 (Mobile Usability), Accessibility (VLIBRAS)
*/

IF DB_ID(N'HEJCareSuite') IS NULL
BEGIN
    THROW 50040, 'Database HEJCareSuite must exist before applying this script.', 1;
END;
GO

USE [HEJCareSuite];
GO

IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
BEGIN
    THROW 50041, 'Table dbo.SchemaVersion must exist before applying this script.', 1;
END;
GO

IF OBJECT_ID(N'dbo.Patient', N'U') IS NULL
BEGIN
    THROW 50042, 'Table dbo.Patient must exist before applying this script.', 1;
END;
GO

-- 1. Staff Care Tasks for Mobile Tracking
IF OBJECT_ID(N'dbo.StaffTask', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.StaffTask
    (
        TaskId UNIQUEIDENTIFIER NOT NULL
            CONSTRAINT DF_StaffTask_TaskId DEFAULT NEWSEQUENTIALID(),
        PatientId UNIQUEIDENTIFIER NOT NULL,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000) NULL,
        Priority INT NOT NULL CONSTRAINT DF_StaffTask_Priority DEFAULT 2, -- 1: Low, 2: Medium, 3: High, 4: Critical
        IsCompleted BIT NOT NULL CONSTRAINT DF_StaffTask_IsCompleted DEFAULT 0,
        AssignedRole NVARCHAR(50) NOT NULL CONSTRAINT DF_StaffTask_AssignedRole DEFAULT N'Nurse',
        DueDateUtc DATETIME2(3) NULL,
        CreatedAtUtc DATETIME2(3) NOT NULL CONSTRAINT DF_StaffTask_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
        CompletedAtUtc DATETIME2(3) NULL,
        CONSTRAINT PK_StaffTask PRIMARY KEY (TaskId),
        CONSTRAINT FK_StaffTask_Patient FOREIGN KEY (PatientId) REFERENCES dbo.Patient(PatientId) ON DELETE CASCADE,
        CONSTRAINT CK_StaffTask_Title_NotBlank CHECK (LEN(LTRIM(RTRIM(Title))) > 0)
    );
END;
GO

-- 2. Critical Emergency Alerts for Mobile Team Notifications
IF OBJECT_ID(N'dbo.CriticalAlert', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.CriticalAlert
    (
        AlertId UNIQUEIDENTIFIER NOT NULL
            CONSTRAINT DF_CriticalAlert_AlertId DEFAULT NEWSEQUENTIALID(),
        PatientId UNIQUEIDENTIFIER NULL,
        Severity NVARCHAR(20) NOT NULL CONSTRAINT CK_CriticalAlert_Severity CHECK (Severity IN (N'INFO', N'WARNING', N'CRITICAL', N'EMERGENCY')),
        Message NVARCHAR(500) NOT NULL,
        IsAcknowledged BIT NOT NULL CONSTRAINT DF_CriticalAlert_IsAcknowledged DEFAULT 0,
        TriggeredAtUtc DATETIME2(3) NOT NULL CONSTRAINT DF_CriticalAlert_TriggeredAtUtc DEFAULT SYSUTCDATETIME(),
        AcknowledgedAtUtc DATETIME2(3) NULL,
        AcknowledgedByUserId UNIQUEIDENTIFIER NULL,
        CONSTRAINT PK_CriticalAlert PRIMARY KEY (AlertId),
        CONSTRAINT FK_CriticalAlert_Patient FOREIGN KEY (PatientId) REFERENCES dbo.Patient(PatientId) ON DELETE SET NULL,
        CONSTRAINT CK_CriticalAlert_Message_NotBlank CHECK (LEN(LTRIM(RTRIM(Message))) > 0)
    );
END;
GO

-- 3. Mobile Offline Sync Audit Log
IF OBJECT_ID(N'dbo.MobileSyncLog', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.MobileSyncLog
    (
        SyncLogId BIGINT IDENTITY(1, 1) NOT NULL,
        DeviceId NVARCHAR(100) NOT NULL,
        UserId UNIQUEIDENTIFIER NOT NULL,
        SyncedRecordsCount INT NOT NULL,
        SyncStatus NVARCHAR(30) NOT NULL DEFAULT N'SUCCESS',
        SyncedAtUtc DATETIME2(3) NOT NULL CONSTRAINT DF_MobileSyncLog_SyncedAtUtc DEFAULT SYSUTCDATETIME(),
        CONSTRAINT PK_MobileSyncLog PRIMARY KEY (SyncLogId)
    );
END;
GO

-- Indexes for performance
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_StaffTask_Patient_Status' AND object_id = OBJECT_ID(N'dbo.StaffTask'))
BEGIN
    CREATE INDEX IX_StaffTask_Patient_Status ON dbo.StaffTask (PatientId, IsCompleted, DueDateUtc);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_CriticalAlert_Unacknowledged' AND object_id = OBJECT_ID(N'dbo.CriticalAlert'))
BEGIN
    CREATE INDEX IX_CriticalAlert_Unacknowledged ON dbo.CriticalAlert (IsAcknowledged, TriggeredAtUtc DESC);
END;
GO

-- Audit Triggers for StaffTask
IF OBJECT_ID(N'dbo.TR_StaffTask_AuditLog', N'TR') IS NULL
BEGIN
    EXEC(N'
        CREATE TRIGGER dbo.TR_StaffTask_AuditLog
        ON dbo.StaffTask
        AFTER INSERT, UPDATE, DELETE
        AS
        BEGIN
            SET NOCOUNT ON;
            INSERT INTO dbo.AuditLog (EntityName, EntityId, ActionName, Details)
            SELECT
                N''StaffTask'',
                COALESCE(inserted.TaskId, deleted.TaskId),
                CASE
                    WHEN inserted.TaskId IS NOT NULL AND deleted.TaskId IS NULL THEN N''INSERT''
                    WHEN inserted.TaskId IS NOT NULL AND deleted.TaskId IS NOT NULL THEN N''UPDATE''
                    ELSE N''DELETE''
                END,
                N''Staff task record changed''
            FROM inserted
            FULL OUTER JOIN deleted ON inserted.TaskId = deleted.TaskId;
        END;
    ');
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE VersionNumber = 7)
BEGIN
    INSERT INTO dbo.SchemaVersion (VersionNumber) VALUES (7);
END;
GO
