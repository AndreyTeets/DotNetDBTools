using System.Data.SqlClient;
using Dapper;

namespace DotNetDBTools.IntegrationTests.MSSQL;

internal static class MSSQLDatabaseHelper
{
    public static void CreateDatabase(string connectionStringWithoutDb, string databaseName)
    {
        using SqlConnection connection = new(connectionStringWithoutDb);
        connection.Execute(
$@"CREATE DATABASE {databaseName};");
    }

    public static void DropDatabaseIfExists(string connectionStringWithoutDb, string databaseName)
    {
        using SqlConnection connection = new(connectionStringWithoutDb);
        connection.Execute(
$@"IF EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = '{databaseName}')
BEGIN
    ALTER DATABASE {databaseName}
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

    DROP DATABASE {databaseName};
END;");
    }

    public static string CreateConnectionString(string connectionStringWithoutDb, string databaseName)
    {
        SqlConnectionStringBuilder connectionStringBuilder = new(connectionStringWithoutDb);
        connectionStringBuilder.InitialCatalog = databaseName;
        return connectionStringBuilder.ConnectionString;
    }
}
