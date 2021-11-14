using System;
using System.Reflection;
using DotNetDBTools.DefinitionParsing.Agnostic;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.DefinitionParsing.MSSQL;
using DotNetDBTools.DefinitionParsing.MySQL;
using DotNetDBTools.DefinitionParsing.PostgreSQL;
using DotNetDBTools.DefinitionParsing.SQLite;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing
{
    public static class DbDefinitionParser
    {
        public static Database CreateDatabaseModel(string dbAssemblyPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            return CreateDatabaseModel(dbAssembly);
        }

        public static Database CreateDatabaseModel(Assembly dbAssembly)
        {
            DatabaseKind dbKind = DbAssemblyInfoHelper.GetDbKind(dbAssembly);
            return dbKind switch
            {
                DatabaseKind.Agnostic => new AgnosticDatabaseModelBuilder().BuildDatabaseModel(dbAssembly),
                DatabaseKind.MSSQL => new MSSQLDatabaseModelBuilder().BuildDatabaseModel(dbAssembly),
                DatabaseKind.MySQL => new MySQLDatabaseModelBuilder().BuildDatabaseModel(dbAssembly),
                DatabaseKind.PostgreSQL => new PostgreSQLDatabaseModelBuilder().BuildDatabaseModel(dbAssembly),
                DatabaseKind.SQLite => new SQLiteDatabaseModelBuilder().BuildDatabaseModel(dbAssembly),
                _ => throw new InvalidOperationException($"Invalid dbKind: {dbKind}"),
            };
        }
    }
}
