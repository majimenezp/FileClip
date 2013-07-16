using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace FileClip.Repositories
{
    public abstract class BaseFileRepository
    {
        protected string storagePath;
        protected string currentPath;

        public abstract StorageMode CurrentStorageMode {get;set;}

        public abstract string CurrentStorageDirectory {get;set;}

        public abstract FileData StoreFile(System.IO.Stream FileContent, string FileName);

        public abstract FileData GetFile(long Id);

        public abstract FileData GetFile(Guid Uid);

        public abstract bool CheckDatabaseSchema();

        public abstract bool MigrateUp();

        protected virtual Stream GetFileFromFileSystem(FileData file)
        {
            var stream = File.OpenRead(Path.Combine(storagePath, file.StoredName));
            return stream;
        }

        public BaseFileRepository()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            currentPath = Path.GetDirectoryName(location);
        }

        protected void CheckDatabaseAndFolder()
        {
            if (!CheckDatabaseSchema())
            {
                MigrateUp();
            }
            if (storagePath == null)
            {
                storagePath = Path.Combine(currentPath, "filesRepo");
            }
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
        }
        
    }
}
