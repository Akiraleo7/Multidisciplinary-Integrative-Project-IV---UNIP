/*
    HEJ Care Suite - Authentication baseline
    Requirement coverage: NFR-01

    Demo credentials after execution:
    Email: admin@hejcare.local
    Password: ChangeMe123!
    Change the demo password before any shared or production deployment.
*/

USE [HEJCareSuite];
GO

IF OBJECT_ID(N'dbo.UserAccount', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserAccount
    (
        UserId UNIQUEIDENTIFIER NOT NULL
            CONSTRAINT DF_UserAccount_UserId DEFAULT NEWSEQUENTIALID(),
        Email NVARCHAR(320) NOT NULL,
        PasswordHash NVARCHAR(256) NOT NULL,
        PasswordSalt NVARCHAR(256) NOT NULL,
        RoleName NVARCHAR(40) NOT NULL,
        IsActive BIT NOT NULL
            CONSTRAINT DF_UserAccount_IsActive DEFAULT 1,
        CreatedAtUtc DATETIME2(3) NOT NULL
            CONSTRAINT DF_UserAccount_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
        CONSTRAINT PK_UserAccount PRIMARY KEY (UserId),
        CONSTRAINT UQ_UserAccount_Email UNIQUE (Email)
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1 FROM dbo.UserAccount
    WHERE Email = N'admin@hejcare.local'
)
BEGIN
    INSERT INTO dbo.UserAccount
        (Email, PasswordHash, PasswordSalt, RoleName)
    VALUES
    (
        N'admin@hejcare.local',
        N'FvVL4cSBjMGGr8vGlslE0s4opxB3K/5RCflrktrqh+Y=',
        N'SEVKQ2FyZVN1aXRlRGVtb1NhbHQ=',
        N'Administrator'
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1 FROM dbo.SchemaVersion WHERE VersionNumber = 3
)
BEGIN
    INSERT INTO dbo.SchemaVersion (VersionNumber) VALUES (3);
END;
GO
