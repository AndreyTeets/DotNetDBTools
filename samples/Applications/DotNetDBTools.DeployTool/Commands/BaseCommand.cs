using System.Data.Common;
using System.Data.SqlClient;
using DotNetDBTools.Deploy;
using Microsoft.Data.Sqlite;
using MySqlConnector;
using Npgsql;

namespace DotNetDBTools.DeployTool.Commands;

internal abstract class BaseCommand
{
    protected IDeployManager CreateDeployManager(Dbms dbms)
    {
        return dbms switch
        {
            Dbms.MSSQL => new MSSQLDeployManager(),
            Dbms.MySQL => new MySQLDeployManager(),
            Dbms.PostgreSQL => new PostgreSQLDeployManager(),
            Dbms.SQLite => new SQLiteDeployManager(),
            _ => throw new Exception($"Invalid dbms '{dbms}'"),
        };
    }

    protected DbConnection CreateDbConnection(Dbms dbms, string connectionString)
    {
        return dbms switch
        {
            Dbms.MSSQL => new SqlConnection(connectionString),
            Dbms.MySQL => new MySqlConnection(connectionString),
            Dbms.PostgreSQL => new NpgsqlConnection(connectionString),
            Dbms.SQLite => new SqliteConnection(connectionString),
            _ => throw new Exception($"Invalid dbms '{dbms}'"),
        };
    }

    protected void SaveToFile(string outputPath, string textContent)
    {
        string fullPath = Path.GetFullPath(outputPath);
        string dirPath = Path.GetDirectoryName(fullPath)!;
        Directory.CreateDirectory(dirPath);
        File.WriteAllText(fullPath, textContent);
    }
}
