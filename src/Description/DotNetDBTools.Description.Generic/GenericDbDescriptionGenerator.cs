using System;
using System.Reflection;
using DotNetDBTools.DbDefinitionAbstractions;
using DotNetDBTools.DefinitionParser.Generic;
using DotNetDBTools.Description.Agnostic;
using DotNetDBTools.Description.MSSQL;
using DotNetDBTools.Description.SQLite;
using DotNetDBTools.Models;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Description.Generic
{
    public class GenericDbDescriptionGenerator
    {
        public static string GenerateDescription(Assembly dbAssembly)
        {
            DbType dbType = GenericDefinitionParser.GetDbType(dbAssembly);
            string dbName = GenericDefinitionParser.GetDbName(dbAssembly);
            IDatabaseInfo<ITableInfo<IColumnInfo>> databaseInfo = GenericDefinitionParser.CreateDatabaseInfo(dbAssembly);
            return dbType switch
            {
                DbType.Agnostic => AgnosticDbDescriptionGenerator.GenerateDescription((AgnosticDatabaseInfo)databaseInfo, dbName),
                DbType.MSSQL => MSSQLDbDescriptionGenerator.GenerateDescription((MSSQLDatabaseInfo)databaseInfo, dbName),
                DbType.SQLite => SQLiteDbDescriptionGenerator.GenerateDescription((SQLiteDatabaseInfo)databaseInfo, dbName),
                _ => throw new InvalidOperationException($"Invalid dbType: {dbType}"),
            };
        }
    }
}
