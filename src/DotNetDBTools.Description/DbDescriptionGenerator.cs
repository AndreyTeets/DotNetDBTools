using System;
using DotNetDBTools.Description.Agnostic;
using DotNetDBTools.Description.MSSQL;
using DotNetDBTools.Description.SQLite;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Shared;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Description
{
    public static class DbDescriptionGenerator
    {
        public static string GenerateDescription(IDatabaseInfo<ITableInfo<IColumnInfo>> databaseInfo)
        {
            return databaseInfo.Kind switch
            {
                DatabaseKind.Agnostic => AgnosticDescriptionSourceGenerator.GenerateDescription((AgnosticDatabaseInfo)databaseInfo),
                DatabaseKind.MSSQL => MSSQLDescriptionSourceGenerator.GenerateDescription((MSSQLDatabaseInfo)databaseInfo),
                DatabaseKind.SQLite => SQLiteDescriptionSourceGenerator.GenerateDescription((SQLiteDatabaseInfo)databaseInfo),
                _ => throw new InvalidOperationException($"Invalid dbKind: {databaseInfo.Kind}"),
            };
        }
    }
}
