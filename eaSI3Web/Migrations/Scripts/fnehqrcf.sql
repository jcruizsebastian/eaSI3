IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [JiraUserName] nvarchar(450) NULL,
    [SI3UserName] nvarchar(450) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);

GO

CREATE TABLE [IssuesCreation] (
    [IssueCreationId] int NOT NULL IDENTITY,
    [JiraKey] nvarchar(max) NULL,
    [SI3Key] nvarchar(max) NULL,
    [CreationDate] datetime2 NOT NULL,
    [CreationResult] int NOT NULL,
    [CreationResultAddtionalInfo] nvarchar(max) NULL,
    [UserId] int NULL,
    CONSTRAINT [PK_IssuesCreation] PRIMARY KEY ([IssueCreationId]),
    CONSTRAINT [FK_IssuesCreation_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Logins] (
    [LoginId] int NOT NULL IDENTITY,
    [UserId] int NULL,
    [ConnectionDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Logins] PRIMARY KEY ([LoginId]),
    CONSTRAINT [FK_Logins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [WorkTracking] (
    [WorkTrackingId] int NOT NULL IDENTITY,
    [UserId] int NULL,
    [TotalHours] int NOT NULL,
    [Week] int NOT NULL,
    [Year] int NOT NULL,
    [TrackingDate] datetime2 NOT NULL,
    [TrackResult] int NOT NULL,
    [TrackResultAddtionalInfo] nvarchar(max) NULL,
    CONSTRAINT [PK_WorkTracking] PRIMARY KEY ([WorkTrackingId]),
    CONSTRAINT [FK_WorkTracking_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_IssuesCreation_UserId] ON [IssuesCreation] ([UserId]);

GO

CREATE INDEX [IX_Logins_UserId] ON [Logins] ([UserId]);

GO

CREATE UNIQUE INDEX [IX_Users_JiraUserName_SI3UserName] ON [Users] ([JiraUserName], [SI3UserName]) WHERE [JiraUserName] IS NOT NULL AND [SI3UserName] IS NOT NULL;

GO

CREATE INDEX [IX_WorkTracking_UserId] ON [WorkTracking] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190307161545_Initial_Migration', N'2.2.2-servicing-10034');

GO

