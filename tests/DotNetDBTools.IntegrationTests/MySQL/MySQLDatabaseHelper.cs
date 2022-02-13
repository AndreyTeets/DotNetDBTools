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

    public static string CreateConnectionString(string connectionStringWithoutDb, string databaseName)
    {
        MySqlConnectionStringBuilder connectionStringBuilder = new(connectionStringWithoutDb);
        connectionStringBuilder.Database = databaseName;
        return connectionStringBuilder.ConnectionString;
    }
}
