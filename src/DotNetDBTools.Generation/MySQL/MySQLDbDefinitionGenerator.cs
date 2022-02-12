using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Generation.MySQL;

internal static class MySQLDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> GenerateDefinition(MySQLDatabase database)
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
