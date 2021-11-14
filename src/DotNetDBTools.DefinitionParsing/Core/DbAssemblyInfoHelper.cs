﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core
{
    internal static class DbAssemblyInfoHelper
    {
        // TODO GetDbInfo (from assembly attributes?) instead of GetDbKind+GetDbName
        public static DatabaseKind GetDbKind(Assembly dbAssembly)
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

        public static string GetDbName(Assembly dbAssembly)
        {
            return dbAssembly.GetName().Name.Replace(".", "");
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
}
