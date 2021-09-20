using System;
using System.Collections.Generic;
using System.IO;
using DotNetDBTools.DefinitionGenerator.Agnostic;
using DotNetDBTools.DefinitionGenerator.Core;
using DotNetDBTools.DefinitionGenerator.MSSQL;
using DotNetDBTools.DefinitionGenerator.SQLite;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionGenerator
{
    public static class DbDefinitionGenerator
    {
        public static void GenerateDefinition(IDatabaseInfo<ITableInfo<IColumnInfo>> databaseInfo, string outputDirectory)
        {
            IEnumerable<DefinitionSourceFile> definitionSourceFiles = GenerateDefinition(databaseInfo);
            foreach (DefinitionSourceFile file in definitionSourceFiles)
            {
                string fullPath = Path.GetFullPath(Path.Combine(outputDirectory, file.RelativePath));
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                File.WriteAllText(fullPath, file.SourceText);
            }
        }

        public static IEnumerable<DefinitionSourceFile> GenerateDefinition(IDatabaseInfo<ITableInfo<IColumnInfo>> databaseInfo)
        {
            IEnumerable<DefinitionSourceFile> definitionSourceFiles = databaseInfo.Kind switch
            {
                DatabaseKind.Agnostic => AgnosticDefinitionGenerator.GenerateDefinition((AgnosticDatabaseInfo)databaseInfo),
                DatabaseKind.MSSQL => MSSQLDefinitionGenerator.GenerateDefinition((MSSQLDatabaseInfo)databaseInfo),
                DatabaseKind.SQLite => SQLiteDefinitionGenerator.GenerateDefinition((SQLiteDatabaseInfo)databaseInfo),
                _ => throw new InvalidOperationException($"Invalid dbKind: {databaseInfo.Kind}"),
            };
            return definitionSourceFiles;
        }
    }
}
