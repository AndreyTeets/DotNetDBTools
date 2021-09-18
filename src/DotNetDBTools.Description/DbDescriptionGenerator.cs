using System;
using DotNetDBTools.Description.Agnostic;
using DotNetDBTools.Description.MSSQL;
using DotNetDBTools.Description.SQLite;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Common;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Description
{
    public class DbDescriptionGenerator
    {
        public static string GenerateDescription(IDatabaseInfo<ITableInfo<IColumnInfo>> databaseInfo)
        {
            return databaseInfo.Type switch
            {
                DatabaseType.Agnostic => AgnosticDescriptionSourceGenerator.GenerateDescription((AgnosticDatabaseInfo)databaseInfo),
                DatabaseType.MSSQL => MSSQLDescriptionSourceGenerator.GenerateDescription((MSSQLDatabaseInfo)databaseInfo),
                DatabaseType.SQLite => SQLiteDescriptionSourceGenerator.GenerateDescription((SQLiteDatabaseInfo)databaseInfo),
                _ => throw new InvalidOperationException($"Invalid dbType: {databaseInfo.Type}"),
            };
        }
    }
}
