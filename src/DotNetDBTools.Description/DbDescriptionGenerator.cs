using System;
using System.Reflection;
using DotNetDBTools.Definition;
using DotNetDBTools.DefinitionParser;
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
        public static string GenerateDescription(Assembly dbAssembly)
        {
            DatabaseType dbType = DbDefinitionParser.GetDbType(dbAssembly);
            string dbName = DbDefinitionParser.GetDbName(dbAssembly);
            IDatabaseInfo<ITableInfo<IColumnInfo>> databaseInfo = DbDefinitionParser.CreateDatabaseInfo(dbAssembly);
            return dbType switch
            {
                DatabaseType.Agnostic => AgnosticDescriptionSourceGenerator.GenerateDescription((AgnosticDatabaseInfo)databaseInfo, dbName),
                DatabaseType.MSSQL => MSSQLDescriptionSourceGenerator.GenerateDescription((MSSQLDatabaseInfo)databaseInfo, dbName),
                DatabaseType.SQLite => SQLiteDescriptionSourceGenerator.GenerateDescription((SQLiteDatabaseInfo)databaseInfo, dbName),
                _ => throw new InvalidOperationException($"Invalid dbType: {dbType}"),
            };
        }
    }
}
