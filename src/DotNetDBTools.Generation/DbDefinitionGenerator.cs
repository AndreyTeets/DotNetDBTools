using System;
using System.Collections.Generic;
using System.IO;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.MSSQL;
using DotNetDBTools.Generation.SQLite;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Generation
{
    public static class DbDefinitionGenerator
    {
        public static void GenerateDefinition(Database database, string outputDirectory)
        {
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
                DatabaseKind.MSSQL => MSSQLDefinitionGenerator.GenerateDefinition((MSSQLDatabase)database),
                DatabaseKind.SQLite => SQLiteDefinitionGenerator.GenerateDefinition((SQLiteDatabase)database),
                _ => throw new InvalidOperationException($"Invalid dbKind: {database.Kind}"),
            };
            return definitionSourceFiles;
        }
    }
}
