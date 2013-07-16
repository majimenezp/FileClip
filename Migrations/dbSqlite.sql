/* Using Connection sqlite from Configuration file C:\TFS\FileClip\Migrations\bin\Debug\Migrations.dll.config */
/* VersionMigration migrating ================================================ */

/* CreateTable VersionInfo */
CREATE TABLE "VersionInfo" ("Version" INTEGER NOT NULL);

/* VersionMigration migrated */

/* VersionUniqueMigration migrating ========================================== */

/* CreateIndex VersionInfo (Version) */
CREATE UNIQUE INDEX "UC_Version" ON "VersionInfo" ("Version" ASC);

/* AlterTable VersionInfo */
/* No SQL statement executed. */

/* CreateColumn VersionInfo AppliedOn DateTime */
ALTER TABLE "VersionInfo" ADD COLUMN "AppliedOn" DATETIME;

/* VersionUniqueMigration migrated */

/* 20130715: Migration0001 migrating ========================================= */

/* CreateTable RepoConfig */
CREATE TABLE "RepoConfig" ("Id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, "Name" TEXT NOT NULL, "FolderPath" TEXT NOT NULL);

/* CreateTable Files */
CREATE TABLE "Files" ("Id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, "UId" UNIQUEIDENTIFIER NOT NULL, "OriginalFileName" TEXT NOT NULL, "CurrentFilename" TEXT NOT NULL, "Version" NUMERIC NOT NULL DEFAULT 1, "Extension" TEXT NOT NULL);

/* CreateIndex Files (UId) */
CREATE UNIQUE INDEX "IX_Files_UId" ON "Files" ("UId" ASC);

/* CreateTable Metadata */
CREATE TABLE "Metadata" ("Id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, "Name" TEXT NOT NULL, "Value" TEXT NOT NULL, "IdFile" INTEGER NOT NULL);

/* CreateForeignKey FK_Metadata_IdFile_Files_Id Metadata(IdFile) Files(Id) */
/* No SQL statement executed. */

/* CreateTable FilesBlobs */
CREATE TABLE "FilesBlobs" ("Id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, "IdFile" INTEGER NOT NULL, "Modified" DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP);

/* CreateForeignKey FK_FilesBlobs_IdFile_Files_Id FilesBlobs(IdFile) Files(Id) */
/* No SQL statement executed. */

INSERT INTO "VersionInfo" ("Version", "AppliedOn") VALUES (20130715, '2013-07-15T21:08:55');
/* 20130715: Migration0001 migrated */

/* Task completed. */
