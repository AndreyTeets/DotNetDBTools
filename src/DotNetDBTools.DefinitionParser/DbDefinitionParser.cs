using System;
using System.Reflection;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.Shared;
using DotNetDBTools.DefinitionParser.MSSQL;
using DotNetDBTools.DefinitionParser.SQLite;
using DotNetDBTools.Models.Shared;

namespace DotNetDBTools.DefinitionParser
{
    public static class DbDefinitionParser
    {
        public static IDatabaseInfo<ITableInfo<IColumnInfo>> CreateDatabaseInfo(Assembly dbAssembly)
        {
            DatabaseType dbType = DbAssemblyInfoHelper.GetDbType(dbAssembly);
            return dbType switch
            {
                DatabaseType.Agnostic => AgnosticDefinitionParser.CreateDatabaseInfo(dbAssembly),
                DatabaseType.MSSQL => MSSQLDefinitionParser.CreateDatabaseInfo(dbAssembly),
                DatabaseType.SQLite => SQLiteDefinitionParser.CreateDatabaseInfo(dbAssembly),
                _ => throw new InvalidOperationException($"Invalid dbType: {dbType}"),
            };
        }
    }
}
