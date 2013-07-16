using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;
namespace FileClip.Migrations
{
    [Migration(20130715)]
    public class Migration0001:Migration
    {
        public override void Down()
        {
            Delete.Table("RepoConfig");
            Delete.Table("Files");
            Delete.Table("Metadata");
        }
        public override void Up()
        {
            Create.Table("RepoConfig")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("FolderPath").AsString(4000).NotNullable();
            Create.Table("Files")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UId").AsGuid().Unique()
                .WithColumn("OriginalFileName").AsString(4000).NotNullable()
                .WithColumn("CurrentFilename").AsString(1000).NotNullable()
                .WithColumn("Version").AsDecimal().WithDefaultValue(1)
                .WithColumn("Extension").AsString().NotNullable();
            Create.Table("Metadata")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(1000).NotNullable()
                .WithColumn("Value").AsString(1000).NotNullable()
                .WithColumn("IdFile").AsInt64().ForeignKey("Files","Id");
            Create.Table("FilesBlobs")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("IdFile").AsInt64().ForeignKey("Files", "Id")
                .WithColumn("Modified").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
        }
    }
}
