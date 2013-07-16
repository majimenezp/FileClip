using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FileClip.Repositories;

namespace FileClip
{
    public class FileRepository
    {
        private BaseFileRepository repo;
        private string name;


        public FileRepository(BaseFileRepository Repository, string Name)
        {
            this.repo = Repository;
            this.name = Name;
        }

        public FileData StoreFile(string filePath)
        {
            var data = File.OpenRead(filePath);
            var fileName = Path.GetFileName(filePath);
            FileData result = repo.StoreFile(data, fileName);
            return result;
        }

        public FileData GetFile(long Id)
        {
            return repo.GetFile(Id);
        }

        public FileData GetFile(Guid Uid)
        {
            return repo.GetFile(Uid);
        }
    }
}
