using System;
using System.IO;
using DotNetDBTools.Description.Agnostic;
using DotNetDBTools.Description.MSSQL;
using DotNetDBTools.Description.SQLite;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Description
{
    public static class DbDescriptionGenerator
    {
        public static void GenerateDescription(DatabaseInfo databaseInfo, string outputPath)
        {
            string generatedDescription = GenerateDescription(databaseInfo);
            string fullPath = Path.GetFullPath(outputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, generatedDescription);
        }

        public static string GenerateDescription(DatabaseInfo databaseInfo)
        {
            return databaseInfo.Kind switch
            {
                DatabaseKind.Agnostic => AgnosticDescriptionSourceGenerator.GenerateDescription((AgnosticDatabaseInfo)databaseInfo),
                DatabaseKind.MSSQL => MSSQLDescriptionSourceGenerator.GenerateDescription((MSSQLDatabaseInfo)databaseInfo),
                DatabaseKind.SQLite => SQLiteDescriptionSourceGenerator.GenerateDescription((SQLiteDatabaseInfo)databaseInfo),
                _ => throw new InvalidOperationException($"Invalid dbKind: {databaseInfo.Kind}"),
            };
        }
    }
}
