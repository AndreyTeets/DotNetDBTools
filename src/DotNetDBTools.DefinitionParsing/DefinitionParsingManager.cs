using System;
using System.IO;
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

public class DefinitionParsingManager : IDefinitionParsingManager
{
    public Assembly LoadDbAssembly(string dbAssemblyPath)
    {
        string fullDbAssemblyPath = Path.GetFullPath(dbAssemblyPath);
        byte[] assemblyBytes = File.ReadAllBytes(fullDbAssemblyPath);
        Assembly dbAssembly = Assembly.Load(assemblyBytes);
        return dbAssembly;
    }

    public Database CreateDbModel(string dbAssemblyPath)
    {
        Assembly dbAssembly = LoadDbAssembly(dbAssemblyPath);
        return CreateDbModel(dbAssembly);
    }

    public Database CreateDbModel(Assembly dbAssembly)
    {
        (DefinitionKind defKind, DatabaseKind dbKind) = DbAssemblyInfoHelper.GetDbKind(dbAssembly);
        return defKind switch
        {
            DefinitionKind.CSharp => BuildDatabaseModelFromCSharpDefinition(dbAssembly, dbKind),
            DefinitionKind.MSSQL => throw new NotImplementedException(),
            DefinitionKind.MySQL => throw new NotImplementedException(),
            DefinitionKind.PostgreSQL => new PostgreSQLDbModelFromSqlDefinitionProvider().CreateDbModel(dbAssembly),
            DefinitionKind.SQLite => new SQLiteDbModelFromSqlDefinitionProvider().CreateDbModel(dbAssembly),
            _ => throw new InvalidOperationException($"Invalid defKind: {defKind}"),
        };
    }

    private Database BuildDatabaseModelFromCSharpDefinition(Assembly dbAssembly, DatabaseKind dbKind)
    {
        return dbKind switch
        {
            DatabaseKind.Agnostic => new AgnosticDatabaseModelProvider().CreateDbModel(dbAssembly),
            DatabaseKind.MSSQL => new MSSQLDbModelFromCSharpDefinitionProvider().CreateDbModel(dbAssembly),
            DatabaseKind.MySQL => new MySQLDbModelFromCSharpDefinitionProvider().CreateDbModel(dbAssembly),
            DatabaseKind.PostgreSQL => new PostgreSQLDbModelFromCSharpDefinitionProvider().CreateDbModel(dbAssembly),
            DatabaseKind.SQLite => new SQLiteDbModelFromCSharpDefinitionProvider().CreateDbModel(dbAssembly),
            _ => throw new InvalidOperationException($"Invalid dbKind: {dbKind}"),
        };
    }
}
