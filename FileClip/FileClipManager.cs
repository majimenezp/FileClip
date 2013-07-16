using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileClip.Repositories;
namespace FileClip
{
    public class FileClipManager
    {
        private static readonly Lazy<FileClipManager> instance = new Lazy<FileClipManager>(() => new FileClipManager());
        private Dictionary<string, FileRepository> repos;
        private FileClipManager()
        {
            repos = new Dictionary<string, FileRepository>();
        }

        public static FileRepository GetRepository()
        {
            if (instance.Value.repos.Keys.Count == 0)
            {
                var repo1 = new SqliteFileRepository();
                var name = instance.Value.CheckName();
                var tmp = new FileRepository(repo1, name);
                instance.Value.repos.Add(name, tmp);
                return tmp;
            }
            else
            {
                return instance.Value.repos.First().Value;
            }
        }

        private string CheckName()
        {
            if (repos.ContainsKey("main"))
            {
                int i = 1;
                while (true)
                {
                    var newName = "alt" + i.ToString();
                    if (!repos.ContainsKey(newName))
                    {
                        return newName;
                    }
                    ++i;
                }
            }
            else
            {
                return "main";
            }
        }

        public static FileRepository GetRepository(string RepositoryName)
        {
            return instance.Value.repos[RepositoryName];
        }

        public static FileRepository GetRepository(BaseFileRepository repository)
        {
            var name = instance.Value.CheckName();
            var tmp = new FileRepository(repository, name);
            instance.Value.repos.Add(name, tmp);
            return tmp;
        }

        public static FileRepository GetRepository(BaseFileRepository repository, StorageMode storageMode)
        {
            var name = instance.Value.CheckName();
            var tmp = new FileRepository(repository, name);
            instance.Value.repos.Add(name, tmp);
            return tmp;
        }

        public static FileRepository GetRepository(BaseFileRepository repository, string newDir)
        {
            var name = instance.Value.CheckName();
            repository.CurrentStorageDirectory = newDir;
            var tmp = new FileRepository(repository, name);
            instance.Value.repos.Add(name, tmp);
            return tmp;
        }
    }
}
