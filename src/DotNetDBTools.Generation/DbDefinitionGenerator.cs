using System;
using System.Collections.Generic;
using System.IO;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.MSSQL;
using DotNetDBTools.Generation.MySQL;
using DotNetDBTools.Generation.PostgreSQL;
using DotNetDBTools.Generation.SQLite;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation;

public static class DbDefinitionGenerator
{
    private const string ProjectNamespace = "MyDatabaseGeneratedDefinition";

    public static void GenerateDefinition(Database database, string outputDirectory)
    {
        if (Directory.Exists(outputDirectory))
            Directory.Delete(outputDirectory, true);

        IEnumerable<DefinitionSourceFile> definitionSourceFiles = GenerateDefinition(database);
        foreach (DefinitionSourceFile file in definitionSourceFiles)
        {
            string fullPath = Path.GetFullPath(Path.Combine(outputDirectory, file.RelativePath));
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, file.SourceText);
        }
    }

    public static IEnumerable<DefinitionSourceFile> GenerateDefinition(Database database)
    {
        IEnumerable<DefinitionSourceFile> definitionSourceFiles = database.Kind switch
        {
            DatabaseKind.MSSQL => MSSQLDefinitionGenerator.GenerateDefinition(database, ProjectNamespace),
            DatabaseKind.MySQL => MySQLDefinitionGenerator.GenerateDefinition(database, ProjectNamespace),
            DatabaseKind.PostgreSQL => PostgreSQLDefinitionGenerator.GenerateDefinition(database, ProjectNamespace),
            DatabaseKind.SQLite => SQLiteDefinitionGenerator.GenerateDefinition(database, ProjectNamespace),
            _ => throw new InvalidOperationException($"Invalid dbKind: {database.Kind}"),
        };
        return definitionSourceFiles;
    }
}
