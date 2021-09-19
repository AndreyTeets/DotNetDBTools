using System.Collections.Generic;
using DotNetDBTools.DefinitionGenerator.Shared;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionGenerator.SQLite
{
    public static class SQLiteDefinitionGenerator
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
