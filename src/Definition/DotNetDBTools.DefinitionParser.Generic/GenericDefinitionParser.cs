using System;
using System.Reflection;
using DotNetDBTools.DbDefinitionAbstractions;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.MSSQL;
using DotNetDBTools.DefinitionParser.SQLite;
using DotNetDBTools.Models;

namespace DotNetDBTools.DefinitionParser.Generic
{
    public static class GenericDefinitionParser
    {
        // TODO GetDbInfo (from assembly attributes?) instead of GetDbType+GetDbName
        public static DbType GetDbType(Assembly dbAssembly)
        {
            if (AgnosticDefinitionParser.IsAgnosticDb(dbAssembly))
                return DbType.Agnostic;
            else if (MSSQLDefinitionParser.IsMSSQLDb(dbAssembly))
                return DbType.MSSQL;
            else if (SQLiteDefinitionParser.IsSQLiteDb(dbAssembly))
                return DbType.SQLite;
            else
                throw new InvalidOperationException("Invalid dbAssembly");
        }

        public static string GetDbName(Assembly dbAssembly)
        {
            DbType dbType = GetDbType(dbAssembly);
            return dbType switch
            {
                DbType.Agnostic => AgnosticDefinitionParser.GetDbName(dbAssembly),
                DbType.MSSQL => MSSQLDefinitionParser.GetDbName(dbAssembly),
                DbType.SQLite => SQLiteDefinitionParser.GetDbName(dbAssembly),
                _ => throw new InvalidOperationException($"Invalid dbType: {dbType}"),
            };
        }

        public static IDatabaseInfo<ITableInfo<IColumnInfo>> CreateDatabaseInfo(Assembly dbAssembly)
        {
            DbType dbType = GetDbType(dbAssembly);
            return dbType switch
            {
                DbType.Agnostic => AgnosticDefinitionParser.CreateDatabaseInfo(dbAssembly),
                DbType.MSSQL => MSSQLDefinitionParser.CreateDatabaseInfo(dbAssembly),
                DbType.SQLite => SQLiteDefinitionParser.CreateDatabaseInfo(dbAssembly),
                _ => throw new InvalidOperationException($"Invalid dbType: {dbType}"),
            };
        }
    }
}
