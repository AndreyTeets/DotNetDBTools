using System;
using System.Collections.Generic;
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

public class GenerationManager : IGenerationManager
{
    public GenerationOptions Options { get; set; }

    public GenerationManager()
        : this(new GenerationOptions())
    {
    }

    public GenerationManager(GenerationOptions options)
    {
        Options = options;
    }

    public void GenerateDescription(Database database, string outputPath)
    {
        string generatedDescription = GenerateDescription(database);
        string fullPath = Path.GetFullPath(outputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        File.WriteAllText(fullPath, generatedDescription);
    }

    public string GenerateDescription(Database database)
    {
        return database.Kind switch
        {
            DatabaseKind.Agnostic => AgnosticDescriptionGenerator.GenerateDescription((AgnosticDatabase)database, Options),
            DatabaseKind.MSSQL => MSSQLDescriptionGenerator.GenerateDescription((MSSQLDatabase)database, Options),
            DatabaseKind.MySQL => MySQLDescriptionGenerator.GenerateDescription((MySQLDatabase)database, Options),
            DatabaseKind.PostgreSQL => PostgreSQLDescriptionGenerator.GenerateDescription((PostgreSQLDatabase)database, Options),
            DatabaseKind.SQLite => SQLiteDescriptionGenerator.GenerateDescription((SQLiteDatabase)database, Options),
            _ => throw new InvalidOperationException($"Invalid dbKind: {database.Kind}"),
        };
    }

    public void GenerateDefinition(Database database, string outputDirectory)
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

    public IEnumerable<DefinitionSourceFile> GenerateDefinition(Database database)
    {
        IEnumerable<DefinitionSourceFile> definitionSourceFiles = database.Kind switch
        {
            DatabaseKind.MSSQL => MSSQLDefinitionGenerator.GenerateDefinition(database, Options.DatabaseName),
            DatabaseKind.MySQL => MySQLDefinitionGenerator.GenerateDefinition(database, Options.DatabaseName),
            DatabaseKind.PostgreSQL => PostgreSQLDefinitionGenerator.GenerateDefinition(database, Options.DatabaseName),
            DatabaseKind.SQLite => SQLiteDefinitionGenerator.GenerateDefinition(database, Options.DatabaseName),
            _ => throw new InvalidOperationException($"Invalid dbKind: {database.Kind}"),
        };
        return definitionSourceFiles;
    }
}
