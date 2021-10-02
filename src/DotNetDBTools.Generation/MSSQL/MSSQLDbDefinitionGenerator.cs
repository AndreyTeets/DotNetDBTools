using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Generation.MSSQL
{
    internal static class MSSQLDefinitionGenerator
    {
        public static IEnumerable<DefinitionSourceFile> GenerateDefinition(MSSQLDatabase database)
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
}
