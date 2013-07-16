FileClip
========

FileClip is a library to implement in a fast and easy way a file repository using a database and a filesystem.

The main idea is in order to save files in a web application on another kind of application, you don't need to worry about tables or entities to save the attached files.


Currently FileClip support Sqlite and Sql Server (2005 and up) to save the files info, and all the file data is saved in the file system.

Examples
--------

#Sqlite
```
var repo1 = FileClipManager.GetRepository(new SqliteFileRepository(), "C:\filerepo");
var fileInfo=repo1.StoreFile("C:\tmp\file.pdf");
var recoveredFile= repo1.GetFile(fileInfo.Id);
```

#Sql server
```
string connectionString = "data source=localhost;integrated security=true;initial catalog=fileclip;";
var repo1 = FileClipManager.GetRepository(SqlServerFileRepository(connectionString), "C:\filerepo");
var fileInfo=repo1.StoreFile("C:\tmp\file.pdf");
var recoveredFile= repo1.GetFile(fileInfo.Id);
```


