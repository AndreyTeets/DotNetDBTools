using System;
using System.Reflection;
using DotNetDBTools.DefinitionParsing.Agnostic;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.DefinitionParsing.MSSQL;
using DotNetDBTools.DefinitionParsing.SQLite;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing
{
    public static class DbDefinitionParser
    {
        public static DatabaseInfo CreateDatabaseInfo(string dbAssemblyPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            return CreateDatabaseInfo(dbAssembly);
        }

        public static DatabaseInfo CreateDatabaseInfo(Assembly dbAssembly)
        {
            DatabaseKind dbKind = DbAssemblyInfoHelper.GetDbKind(dbAssembly);
            return dbKind switch
            {
                DatabaseKind.Agnostic => new AgnosticDatabaseModelBuilder().BuildDatabaseModel(dbAssembly),
                DatabaseKind.MSSQL => new MSSQLDatabaseModelBuilder().BuildDatabaseModel(dbAssembly),
                DatabaseKind.SQLite => new SQLiteDatabaseModelBuilder().BuildDatabaseModel(dbAssembly),
                _ => throw new InvalidOperationException($"Invalid dbKind: {dbKind}"),
            };
        }
    }
}
