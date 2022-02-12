using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Definition;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal static class DbAssemblyInfoHelper
{
    public static (DefinitionKind, DatabaseKind) GetDbKind(Assembly dbAssembly)
    {
        DefinitionSettingsAttribute definitionSettings = dbAssembly
            .GetCustomAttributes<DefinitionSettingsAttribute>()
            .SingleOrDefault();

        DefinitionKind definitionKind;
        DatabaseKind databaseKind;
        if (definitionSettings is not null)
        {
            definitionKind = definitionSettings.DefinitionKind;
            databaseKind = definitionSettings.DefinitionKind switch
            {
                DefinitionKind.CSharp => DeriveDbKindFromAssemblyTypes(dbAssembly),
                DefinitionKind.MSSQL => DatabaseKind.MSSQL,
                DefinitionKind.MySQL => DatabaseKind.MySQL,
                DefinitionKind.PostgreSQL => DatabaseKind.PostgreSQL,
                DefinitionKind.SQLite => DatabaseKind.SQLite,
                _ => throw new Exception($"Invalid definition kind: {definitionSettings.DefinitionKind}"),
            };
        }
        else
        {
            definitionKind = DefinitionKind.CSharp;
            databaseKind = DeriveDbKindFromAssemblyTypes(dbAssembly);
        }

        return (definitionKind, databaseKind);
    }

    public static string GetDbName(Assembly dbAssembly)
    {
        return dbAssembly.GetName().Name.Replace(".", "");
    }

    private static DatabaseKind DeriveDbKindFromAssemblyTypes(Assembly dbAssembly)
    {
        bool isAgnosticDb = IsAgnosticDb(dbAssembly);
        bool isMSSQLDb = IsMSSQLDb(dbAssembly);
        bool isMySQLDb = IsMySQLDb(dbAssembly);
        bool isPostgreSQLDb = IsPostgreSQLDb(dbAssembly);
        bool isSQLiteDb = IsSQLiteDb(dbAssembly);

        List<bool> assertions = new() { isAgnosticDb, isMSSQLDb, isMySQLDb, isPostgreSQLDb, isSQLiteDb };
        int trueAssertionsCount = assertions.Count(x => x);
        if (trueAssertionsCount != 1)
            throw new InvalidOperationException($"Invalid dbAssembly: failed to uniquely identify db kind ({trueAssertionsCount})");

        if (isAgnosticDb)
            return DatabaseKind.Agnostic;
        else if (isMSSQLDb)
            return DatabaseKind.MSSQL;
        else if (isMySQLDb)
            return DatabaseKind.MySQL;
        else if (isPostgreSQLDb)
            return DatabaseKind.PostgreSQL;
        else if (isSQLiteDb)
            return DatabaseKind.SQLite;
        else
            throw new InvalidOperationException("Invalid dbAssembly: unknown DatabaseKind");
    }

    private static bool IsAgnosticDb(Assembly dbAssembly)
    {
        return dbAssembly
            .GetTypes()
            .Any(x => x.GetInterfaces()
                .Any(y => y == typeof(Definition.Agnostic.ITable)));
    }

    private static bool IsMSSQLDb(Assembly dbAssembly)
    {
        return dbAssembly
            .GetTypes()
            .Any(x => x.GetInterfaces()
                .Any(y => y == typeof(Definition.MSSQL.ITable)));
    }

    private static bool IsMySQLDb(Assembly dbAssembly)
    {
        return dbAssembly
            .GetTypes()
            .Any(x => x.GetInterfaces()
                .Any(y => y == typeof(Definition.MySQL.ITable)));
    }

    private static bool IsPostgreSQLDb(Assembly dbAssembly)
    {
        return dbAssembly
            .GetTypes()
            .Any(x => x.GetInterfaces()
                .Any(y => y == typeof(Definition.PostgreSQL.ITable)));
    }

    private static bool IsSQLiteDb(Assembly dbAssembly)
    {
        return dbAssembly
            .GetTypes()
            .Any(x => x.GetInterfaces()
                .Any(y => y == typeof(Definition.SQLite.ITable)));
    }
}
