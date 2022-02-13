using Dapper;
using Npgsql;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

internal static class PostgreSQLDatabaseHelper
{
    public static void CreateDatabase(string connectionStringWithoutDb, string databaseName)
    {
        using NpgsqlConnection connection = new(connectionStringWithoutDb);
        connection.Execute(
$@"CREATE DATABASE ""{databaseName}"";");
    }

    public static void DropDatabaseIfExists(string connectionStringWithoutDb, string databaseName)
    {
        using NpgsqlConnection connection = new(connectionStringWithoutDb);
        connection.Execute(
$@"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{databaseName}';
DROP DATABASE IF EXISTS ""{databaseName}"";");
    }

    public static string CreateConnectionString(string connectionStringWithoutDb, string databaseName)
    {
        NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionStringWithoutDb);
        connectionStringBuilder.Database = databaseName;
        return connectionStringBuilder.ConnectionString;
    }
}
