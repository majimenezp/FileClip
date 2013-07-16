using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileClip;
using FileClip.Repositories;
using System.IO;
namespace Tests
{
    [TestClass]
    public class Test_sqlserver_repo
    {
        [TestMethod]
        public void Create_sqlserver_repo()
        {
            string newDir = Path.Combine(Helper.GetCurrentFolder(), "TestDir01");
            if (!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            }

            string connectionString = "data source=localhost;integrated security=true;initial catalog=fileclip;";
            var repo1 = FileClipManager.GetRepository(new SqlServerFileRepository(connectionString), newDir);
            var exfilePath = Helper.GetCurrentFolder() + "\\TestFiles\\testDoc1.pdf";
            var fileInfo=repo1.StoreFile(exfilePath);
            var fileInfo2=repo1.GetFile(fileInfo.Id);

        }
    }
}
