using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using confMan = System.Configuration.ConfigurationManager;
using System.Reflection;
namespace FileClip.Repositories
{
    public class SqlServerFileRepository : BaseFileRepository
    {
        SqlConnection connection;
        private StorageMode storageMode = StorageMode.Folder;
        public SqlServerFileRepository()
            : base()
        {
            var strConn = confMan.AppSettings[0];
            connection = new SqlConnection(strConn);
            CheckDatabaseAndFolder();
        }
        public SqlServerFileRepository(string connectionString)
            : base()
        {
            connection = new SqlConnection(connectionString);
            CheckDatabaseAndFolder();
        }

        public override FileData StoreFile(Stream FileContent, string fileName)
        {
            FileData data = new FileData();
            data.Id = 0;
            data.UId = Guid.NewGuid();
            data.OriginalName = Path.GetFileName(fileName);
            data.Extension = Path.GetExtension(fileName);
            data.StoredName = data.UId.ToString() + data.Extension;
            FileContent.Seek(0, SeekOrigin.Begin);
            if (storageMode == StorageMode.Folder)
            {
                data.Id = SaveInFileSystem(FileContent,data);
            }
            return data;
        }

        public override bool CheckDatabaseSchema()
        {
            int tableNumber = 0;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(1) FROM INFORMATION_SCHEMA.TABLES where lower(TABLE_NAME) in ('files','filesblobs','metadata','repoconfig')";
                connection.Open();
                tableNumber = (int)command.ExecuteScalar();
                connection.Close();
            }
            return tableNumber == 4;
        }

        private long SaveInFileSystem(Stream FileContent, FileData fileData)
        {
            var fileStr = File.Create(Path.Combine(storagePath, fileData.StoredName));
            FileContent.CopyTo(fileStr);
            fileStr.Close();
            fileStr.Dispose();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Files (UId,OriginalFileName,CurrentFilename,Version,Extension) " +
                " OUTPUT INSERTED.Id VALUES (@UId,@OriginalFileName,@CurrentFilename,1,@Extension)";
                command.Parameters.AddWithValue("UId", fileData.UId);
                command.Parameters.AddWithValue("OriginalFileName",fileData.OriginalName);
                command.Parameters.AddWithValue("CurrentFilename", fileData.StoredName);
                command.Parameters.AddWithValue("Extension",fileData.Extension);
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    command.Transaction = trans;
                    fileData.Id = (long)command.ExecuteScalar();
                    trans.Commit();
                }
                connection.Close();
            }
            return fileData.Id;
        }

        public override FileData GetFile(long Id)
        {
            FileData file = new FileData();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Id,UId,OriginalFileName,CurrentFilename,Version,Extension FROM Files where Id=@id";
                command.Parameters.AddWithValue("id", Id);
                connection.Open();
                using (var reader = command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess))
                {
                    if (reader.Read())
                    {
                        file.Id = reader.GetInt64(0);
                        file.UId = reader.GetGuid(1);
                        file.OriginalName = reader.GetString(2);
                        file.StoredName = reader.GetString(3);
                        file.Version = reader.GetDecimal(4);
                        file.Extension = reader.GetString(5);
                    }
                    else
                    {
                        file = null;
                    }
                }
                connection.Close();
            }
            file.FileStream = GetFileFromFileSystem(file);
            return file;
        }

        public override FileData GetFile(Guid Uid)
        {
            FileData file = new FileData();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Id,UId,OriginalFileName,CurrentFilename,Version,Extension FROM Files where UId=@uid";
                command.Parameters.AddWithValue("uid", Uid);
                connection.Open();
                using (var reader = command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess))
                {
                    if (reader.Read())
                    {
                        file.Id = reader.GetInt64(0);
                        file.UId = reader.GetGuid(1);
                        file.OriginalName = reader.GetString(2);
                        file.StoredName = reader.GetString(3);
                        file.Version = reader.GetDecimal(4);
                        file.Extension = reader.GetString(5);
                    }
                    else
                    {
                        file = null;
                    }
                }
                connection.Close();
            }
            file.FileStream = GetFileFromFileSystem(file);
            return file;
        }


        public override bool MigrateUp()
        {
            int changes = 0;
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = Properties.Resources.dbSqlServer;
                changes = command.ExecuteNonQuery();
                connection.Close();
            }
            return changes > 0;
        }

        public override StorageMode CurrentStorageMode
        {
            get
            {
                return storageMode;
            }
            set
            {
                storageMode = value;
            }
        }

        public override string CurrentStorageDirectory
        {
            get
            {
                return storagePath;
            }
            set
            {
                storageMode = StorageMode.Folder;
                storagePath = value;
            }
        }
    }

}
