using System.IO;

namespace DotNetDBTools.IntegrationTests.SQLite;

internal static class SQLiteDatabaseHelper
{
    public static void CreateDatabase(string dbFilePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));
    }

    public static void DropDatabaseIfExists(string dbFilePath)
    {
        if (File.Exists(dbFilePath))
            File.Delete(dbFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));
    }
}
