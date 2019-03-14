ALTER TABLE [WorkTracking] ADD [Submit] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190312093552_Submit_Work', N'2.2.2-servicing-10034');

GO

