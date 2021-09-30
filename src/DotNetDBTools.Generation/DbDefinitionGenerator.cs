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
        public static void GenerateDefinition(DatabaseInfo databaseInfo, string outputDirectory)
        {
            IEnumerable<DefinitionSourceFile> definitionSourceFiles = GenerateDefinition(databaseInfo);
            foreach (DefinitionSourceFile file in definitionSourceFiles)
            {
                string fullPath = Path.GetFullPath(Path.Combine(outputDirectory, file.RelativePath));
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                File.WriteAllText(fullPath, file.SourceText);
            }
        }

        public static IEnumerable<DefinitionSourceFile> GenerateDefinition(DatabaseInfo databaseInfo)
        {
            IEnumerable<DefinitionSourceFile> definitionSourceFiles = databaseInfo.Kind switch
            {
                DatabaseKind.MSSQL => MSSQLDefinitionGenerator.GenerateDefinition((MSSQLDatabaseInfo)databaseInfo),
                DatabaseKind.SQLite => SQLiteDefinitionGenerator.GenerateDefinition((SQLiteDatabaseInfo)databaseInfo),
                _ => throw new InvalidOperationException($"Invalid dbKind: {databaseInfo.Kind}"),
            };
            return definitionSourceFiles;
        }
    }
}
