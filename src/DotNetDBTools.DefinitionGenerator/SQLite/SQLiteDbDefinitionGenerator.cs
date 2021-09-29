using System.Collections.Generic;
using DotNetDBTools.DefinitionGenerator.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionGenerator.SQLite
{
    internal static class SQLiteDefinitionGenerator
    {
        public static IEnumerable<DefinitionSourceFile> GenerateDefinition(SQLiteDatabaseInfo databaseInfo)
        {
            return new List<DefinitionSourceFile>()
            {
                new DefinitionSourceFile()
                {
                    RelativePath = "Tables/Table1.cs",
                    SourceText = databaseInfo.Name,
                }
            };
        }
    }
}
