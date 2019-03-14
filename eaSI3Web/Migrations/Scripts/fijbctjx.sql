ALTER TABLE [Users] ADD [JiraPassword] nvarchar(max) NULL;

GO

ALTER TABLE [Users] ADD [SI3Password] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190313093601_UserPassword', N'2.2.2-servicing-10034');

GO

