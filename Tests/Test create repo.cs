using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileClip;
using FileClip.Repositories;
using System.IO;
namespace Tests
{
    [TestClass]
    public class Test_create_repositories
    {
        [TestMethod]
        public void Create_default_repo()
        {
            var filePath=Helper.GetCurrentFolder()+"\\filedb.s3db";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            string newDir=Path.Combine(Helper.GetCurrentFolder(),"TestDir01");
            if(!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            }

            var repo1 = FileClipManager.GetRepository(new SqliteFileRepository(), newDir);
            
            var exfilePath=Helper.GetCurrentFolder() +"\\TestFiles\\testDoc1.pdf";
            repo1.StoreFile(exfilePath);
            var fileInfo = repo1.StoreFile(exfilePath);
            var fileInfo2 = repo1.GetFile(fileInfo.Id);
        }

       
    }
}
