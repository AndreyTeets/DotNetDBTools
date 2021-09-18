using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Models.Common;

namespace DotNetDBTools.DefinitionParser.Common
{
    public static class DbAssemblyInfoHelper
    {
        // TODO GetDbInfo (from assembly attributes?) instead of GetDbType+GetDbName
        public static DatabaseType GetDbType(Assembly dbAssembly)
        {
            bool isAgnosticDb = IsAgnosticDb(dbAssembly);
            bool isMSSQLDb = IsMSSQLDb(dbAssembly);
            bool isSQLiteDb = IsSQLiteDb(dbAssembly);

            List<bool> assertions = new() { isAgnosticDb, isMSSQLDb, isSQLiteDb };
            int trueAssertionsCount = assertions.Count(x => x);
            if (trueAssertionsCount != 1)
                throw new InvalidOperationException($"Invalid dbAssembly: failed to uniquely identify db type ({trueAssertionsCount})");

            if (isAgnosticDb)
                return DatabaseType.Agnostic;
            else if (isMSSQLDb)
                return DatabaseType.MSSQL;
            else if (isSQLiteDb)
                return DatabaseType.SQLite;
            else
                throw new InvalidOperationException("Invalid dbAssembly: unknown DatabaseType (this should never happen)");
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
