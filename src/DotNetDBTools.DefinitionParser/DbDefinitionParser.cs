using System;
using System.Reflection;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.DefinitionParser.MSSQL;
using DotNetDBTools.DefinitionParser.SQLite;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParser
{
    public static class DbDefinitionParser
    {
        public static IDatabaseInfo<ITableInfo<IColumnInfo>> CreateDatabaseInfo(Assembly dbAssembly)
        {
            DatabaseKind dbKind = DbAssemblyInfoHelper.GetDbKind(dbAssembly);
            return dbKind switch
            {
                DatabaseKind.Agnostic => AgnosticDefinitionParser.CreateDatabaseInfo(dbAssembly),
                DatabaseKind.MSSQL => MSSQLDefinitionParser.CreateDatabaseInfo(dbAssembly),
                DatabaseKind.SQLite => SQLiteDefinitionParser.CreateDatabaseInfo(dbAssembly),
                _ => throw new InvalidOperationException($"Invalid dbKind: {dbKind}"),
            };
        }
    }
}
