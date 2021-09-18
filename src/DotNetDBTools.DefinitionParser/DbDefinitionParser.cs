using System;
using System.Reflection;
using DotNetDBTools.Definition;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.MSSQL;
using DotNetDBTools.DefinitionParser.SQLite;
using DotNetDBTools.Models.Common;

namespace DotNetDBTools.DefinitionParser
{
    public static class DbDefinitionParser
    {
        // TODO GetDbInfo (from assembly attributes?) instead of GetDbType+GetDbName
        public static DatabaseType GetDbType(Assembly dbAssembly)
        {
            if (AgnosticDefinitionParser.IsAgnosticDb(dbAssembly))
                return DatabaseType.Agnostic;
            else if (MSSQLDefinitionParser.IsMSSQLDb(dbAssembly))
                return DatabaseType.MSSQL;
            else if (SQLiteDefinitionParser.IsSQLiteDb(dbAssembly))
                return DatabaseType.SQLite;
            else
                throw new InvalidOperationException("Invalid dbAssembly");
        }

        public static string GetDbName(Assembly dbAssembly)
        {
            DatabaseType dbType = GetDbType(dbAssembly);
            return dbType switch
            {
                DatabaseType.Agnostic => AgnosticDefinitionParser.GetDbName(dbAssembly),
                DatabaseType.MSSQL => MSSQLDefinitionParser.GetDbName(dbAssembly),
                DatabaseType.SQLite => SQLiteDefinitionParser.GetDbName(dbAssembly),
                _ => throw new InvalidOperationException($"Invalid dbType: {dbType}"),
            };
        }

        public static IDatabaseInfo<ITableInfo<IColumnInfo>> CreateDatabaseInfo(Assembly dbAssembly)
        {
            DatabaseType dbType = GetDbType(dbAssembly);
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
