/* Using Connection sqlserver from Configuration file C:\TFS\FileClip\Migrations\bin\Debug\Migrations.dll.config */
/* VersionMigration migrating ================================================ */

/* Beginning Transaction */
/* CreateTable VersionInfo */
CREATE TABLE [dbo].[VersionInfo] ([Version] BIGINT NOT NULL)

/* Committing Transaction */
/* VersionMigration migrated */

/* VersionUniqueMigration migrating ========================================== */

/* Beginning Transaction */
/* CreateIndex VersionInfo (Version) */
CREATE UNIQUE CLUSTERED INDEX [UC_Version] ON [dbo].[VersionInfo] ([Version] ASC)

/* AlterTable VersionInfo */
/* No SQL statement executed. */

/* CreateColumn VersionInfo AppliedOn DateTime */
ALTER TABLE [dbo].[VersionInfo] ADD [AppliedOn] DATETIME

/* Committing Transaction */
/* VersionUniqueMigration migrated */

/* 20130715: Migration0001 migrating ========================================= */

/* Beginning Transaction */
/* CreateTable RepoConfig */
CREATE TABLE [dbo].[RepoConfig] ([Id] INT NOT NULL IDENTITY(1,1), [Name] NVARCHAR(100) NOT NULL, [FolderPath] NVARCHAR(4000) NOT NULL, CONSTRAINT [PK_RepoConfig] PRIMARY KEY ([Id]))

/* CreateTable Files */
CREATE TABLE [dbo].[Files] ([Id] BIGINT NOT NULL IDENTITY(1,1), [UId] UNIQUEIDENTIFIER NOT NULL, [OriginalFileName] NVARCHAR(4000) NOT NULL, [CurrentFilename] NVARCHAR(1000) NOT NULL, [Version] DECIMAL(19,5) NOT NULL CONSTRAINT [DF_Files_Version] DEFAULT 1, [Extension] NVARCHAR(255) NOT NULL, CONSTRAINT [PK_Files] PRIMARY KEY ([Id]))

/* CreateIndex Files (UId) */
CREATE UNIQUE INDEX [IX_Files_UId] ON [dbo].[Files] ([UId] ASC)

/* CreateTable Metadata */
CREATE TABLE [dbo].[Metadata] ([Id] BIGINT NOT NULL IDENTITY(1,1), [Name] NVARCHAR(1000) NOT NULL, [Value] NVARCHAR(1000) NOT NULL, [IdFile] BIGINT NOT NULL, CONSTRAINT [PK_Metadata] PRIMARY KEY ([Id]))

/* CreateForeignKey FK_Metadata_IdFile_Files_Id Metadata(IdFile) Files(Id) */
ALTER TABLE [dbo].[Metadata] ADD CONSTRAINT [FK_Metadata_IdFile_Files_Id] FOREIGN KEY ([IdFile]) REFERENCES [dbo].[Files] ([Id])

/* CreateTable FilesBlobs */
CREATE TABLE [dbo].[FilesBlobs] ([Id] BIGINT NOT NULL IDENTITY(1,1), [IdFile] BIGINT NOT NULL, [Modified] DATETIME NOT NULL CONSTRAINT [DF_FilesBlobs_Modified] DEFAULT GETDATE(), CONSTRAINT [PK_FilesBlobs] PRIMARY KEY ([Id]))

/* CreateForeignKey FK_FilesBlobs_IdFile_Files_Id FilesBlobs(IdFile) Files(Id) */
ALTER TABLE [dbo].[FilesBlobs] ADD CONSTRAINT [FK_FilesBlobs_IdFile_Files_Id] FOREIGN KEY ([IdFile]) REFERENCES [dbo].[Files] ([Id])

INSERT INTO [dbo].[VersionInfo] ([Version], [AppliedOn]) VALUES (20130715, '2013-07-15T21:08:48')
/* Committing Transaction */
/* 20130715: Migration0001 migrated */

/* Task completed. */
