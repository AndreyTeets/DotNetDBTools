using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Generation.SQLite;

internal static class SQLiteDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> GenerateDefinition(SQLiteDatabase database)
    {
        return new List<DefinitionSourceFile>()
        {
            new DefinitionSourceFile()
            {
                RelativePath = "Tables/Table1.cs",
                SourceText = database.Name,
            }
        };
    }
}
