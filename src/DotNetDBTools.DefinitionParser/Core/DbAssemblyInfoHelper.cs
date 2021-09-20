using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParser.Core
{
    public static class DbAssemblyInfoHelper
    {
        // TODO GetDbInfo (from assembly attributes?) instead of GetDbKind+GetDbName
        public static DatabaseKind GetDbKind(Assembly dbAssembly)
        {
            bool isAgnosticDb = IsAgnosticDb(dbAssembly);
            bool isMSSQLDb = IsMSSQLDb(dbAssembly);
            bool isSQLiteDb = IsSQLiteDb(dbAssembly);

            List<bool> assertions = new() { isAgnosticDb, isMSSQLDb, isSQLiteDb };
            int trueAssertionsCount = assertions.Count(x => x);
            if (trueAssertionsCount != 1)
                throw new InvalidOperationException($"Invalid dbAssembly: failed to uniquely identify db kind ({trueAssertionsCount})");

            if (isAgnosticDb)
                return DatabaseKind.Agnostic;
            else if (isMSSQLDb)
                return DatabaseKind.MSSQL;
            else if (isSQLiteDb)
                return DatabaseKind.SQLite;
            else
                throw new InvalidOperationException("Invalid dbAssembly: unknown DatabaseKind (this should never happen)");
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

        private static bool IsSQLiteDb(Assembly dbAssembly)
        {
            return dbAssembly
                .GetTypes()
                .Any(x => x.GetInterfaces()
                    .Any(y => y == typeof(Definition.SQLite.ITable)));
        }
    }
}
