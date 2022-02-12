using System;
using System.Reflection;
using DotNetDBTools.Definition;
using DotNetDBTools.DefinitionParsing.Agnostic;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.DefinitionParsing.MSSQL;
using DotNetDBTools.DefinitionParsing.MySQL;
using DotNetDBTools.DefinitionParsing.PostgreSQL;
using DotNetDBTools.DefinitionParsing.SQLite;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing;

public static class DbDefinitionParser
{
    public static Database CreateDatabaseModel(string dbAssemblyPath)
    {
        Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
        return CreateDatabaseModel(dbAssembly);
    }

    public static Database CreateDatabaseModel(Assembly dbAssembly)
    {
        (DefinitionKind defKind, DatabaseKind dbKind) = DbAssemblyInfoHelper.GetDbKind(dbAssembly);
        return defKind switch
        {
            DefinitionKind.CSharp => BuildDatabaseModelFromCSharpDefinition(dbAssembly, dbKind),
            DefinitionKind.MSSQL => throw new NotImplementedException(),
            DefinitionKind.MySQL => throw new NotImplementedException(),
            DefinitionKind.PostgreSQL => throw new NotImplementedException(),
            DefinitionKind.SQLite => new SQLiteDbModelFromSqlDefinitionBuilder().BuildDatabaseModel(dbAssembly),
            _ => throw new InvalidOperationException($"Invalid defKind: {defKind}"),
        };
    }

    private static Database BuildDatabaseModelFromCSharpDefinition(Assembly dbAssembly, DatabaseKind dbKind)
    {
        return dbKind switch
        {
            DatabaseKind.Agnostic => new AgnosticDatabaseModelBuilder().BuildDatabaseModel(dbAssembly),
            DatabaseKind.MSSQL => new MSSQLDbModelFromCSharpDefinitionBuilder().BuildDatabaseModel(dbAssembly),
            DatabaseKind.MySQL => new MySQLDbModelFromCSharpDefinitionBuilder().BuildDatabaseModel(dbAssembly),
            DatabaseKind.PostgreSQL => new PostgreSQLDbModelFromCSharpDefinitionBuilder().BuildDatabaseModel(dbAssembly),
            DatabaseKind.SQLite => new SQLiteDbModelFromCSharpDefinitionBuilder().BuildDatabaseModel(dbAssembly),
            _ => throw new InvalidOperationException($"Invalid dbKind: {dbKind}"),
        };
    }
}
