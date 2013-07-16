using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace FileClip.Repositories
{
    public class SqliteFileRepository : BaseFileRepository
    {
        private string connString;
        private StorageMode storageMode = StorageMode.Folder;
        private const string connStringFormat = "data source={0};version=3;Pooling=True;Max Pool Size=100;";
        private string fileName = "filedb.s3db";
        private SQLiteConnection connection;

        public SqliteFileRepository():base()
        {
            connString = string.Format(connStringFormat, Path.Combine(currentPath, fileName));
            connection = new SQLiteConnection(connString);
            CheckDatabaseAndFolder();
        }

        public override FileData StoreFile(Stream FileContent, string fileName)
        {
            FileData data = new FileData();
            data.Id = 0;
            data.UId=Guid.NewGuid();
            data.OriginalName=Path.GetFileName(fileName);
            data.Extension = Path.GetExtension(fileName);
            data.StoredName = data.UId.ToString() + data.Extension;
            FileContent.Seek(0, SeekOrigin.Begin);
            if (storageMode == StorageMode.Folder)
            {
                data.Id = SaveInFileSystem(FileContent, data.UId, data.OriginalName, data.Extension, data.StoredName);
            }
            return data;
        }

        private long SaveInFileSystem(Stream FileContent, Guid uid, string oname, string ext, string cname)
        {
            long Id=0;
            var fileStr = File.Create(Path.Combine(storagePath, cname));
            FileContent.CopyTo(fileStr);
            fileStr.Close();
            fileStr.Dispose();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "insert into files (Uid,OriginalFilename,CurrentFilename,version,extension) values(@uid,@oname,@cname,1,@ext);select last_insert_rowid();";
                command.Parameters.AddWithValue("uid", uid);
                command.Parameters.AddWithValue("oname", oname);
                command.Parameters.AddWithValue("cname", cname);
                command.Parameters.AddWithValue("ext", ext);
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    command.Transaction = trans;
                    Id = (long)command.ExecuteScalar();
                    trans.Commit();
                }
                connection.Close();
            }
            return Id;
        }

        public override bool CheckDatabaseSchema()
        {
            long tableNumber = 0;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT count(1) FROM sqlite_master WHERE type='table' AND lower(name) in ('repoconfig','files','metadata','filesblobs');)";
                connection.Open();
                tableNumber = (long)command.ExecuteScalar();
                connection.Close();
            }
            return tableNumber == 4;
        }

        public override bool MigrateUp()
        {
            int changes = 0;
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = Properties.Resources.dbSqlite;
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
    }
}
