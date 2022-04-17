using System;
using System.IO;
using DotNetDBTools.Generation.Agnostic;
using DotNetDBTools.Generation.MSSQL;
using DotNetDBTools.Generation.MySQL;
using DotNetDBTools.Generation.PostgreSQL;
using DotNetDBTools.Generation.SQLite;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MySQL;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Generation;

public static class DbDescriptionGenerator
{
    public static void GenerateDescription(Database database, GenerationOptions options, string outputPath)
    {
        string generatedDescription = GenerateDescription(database, options);
        string fullPath = Path.GetFullPath(outputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        File.WriteAllText(fullPath, generatedDescription);
    }

    public static string GenerateDescription(Database database, GenerationOptions options)
    {
        return database.Kind switch
        {
            DatabaseKind.Agnostic => AgnosticDescriptionGenerator.GenerateDescription((AgnosticDatabase)database, options),
            DatabaseKind.MSSQL => MSSQLDescriptionGenerator.GenerateDescription((MSSQLDatabase)database, options),
            DatabaseKind.MySQL => MySQLDescriptionGenerator.GenerateDescription((MySQLDatabase)database, options),
            DatabaseKind.PostgreSQL => PostgreSQLDescriptionGenerator.GenerateDescription((PostgreSQLDatabase)database, options),
            DatabaseKind.SQLite => SQLiteDescriptionGenerator.GenerateDescription((SQLiteDatabase)database, options),
            _ => throw new InvalidOperationException($"Invalid dbKind: {database.Kind}"),
        };
    }
}
