using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL;

internal static class PostgreSQLDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> GenerateDefinition(PostgreSQLDatabase database)
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
