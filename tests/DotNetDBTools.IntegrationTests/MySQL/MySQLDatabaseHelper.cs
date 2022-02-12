using Dapper;
using MySqlConnector;

namespace DotNetDBTools.IntegrationTests.MySQL;

internal static class MySQLDatabaseHelper
{
    public static void CreateDatabase(string connectionStringWithoutDb, string databaseName)
    {
        using MySqlConnection connection = new(connectionStringWithoutDb);
        connection.Execute(
$@"CREATE DATABASE `{databaseName}`;");
    }

    public static void DropDatabaseIfExists(string connectionStringWithoutDb, string databaseName)
    {
        using MySqlConnection connection = new(connectionStringWithoutDb);
        connection.Execute(
$@"DROP DATABASE IF EXISTS `{databaseName}`;");
    }
}
