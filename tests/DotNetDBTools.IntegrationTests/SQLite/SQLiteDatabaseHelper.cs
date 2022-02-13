using System.IO;

namespace DotNetDBTools.IntegrationTests.SQLite;

internal static class SQLiteDatabaseHelper
{
    public static void CreateDatabase(string dbFilesFolder, string databaseName)
    {
        string dbFilePath = GetDbFilePath(dbFilesFolder, databaseName);
        Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));
    }

    public static void DropDatabaseIfExists(string dbFilesFolder, string databaseName)
    {
        string dbFilePath = GetDbFilePath(dbFilesFolder, databaseName);
        if (File.Exists(dbFilePath))
            File.Delete(dbFilePath);
    }

    public static string CreateConnectionString(string dbFilesFolder, string databaseName)
    {
        string dbFilePath = GetDbFilePath(dbFilesFolder, databaseName);
        return $@"DataSource={dbFilePath};Mode=ReadWriteCreate;";
    }

    private static string GetDbFilePath(string dbFilesFolder, string databaseName)
    {
        return $"{dbFilesFolder}/{databaseName}.db";
    }
}
