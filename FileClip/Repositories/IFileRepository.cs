using FileClip.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileClip
{
    public interface IFileRepository
    {
        StorageMode CurrentStorageMode { get; set; }
        string CurrentStorageDirectory { get; set; }
        Guid StoreFile(Stream FileContent,string FileName);
        bool CheckDatabaseSchema();
        bool MigrateUp();
    }
}
