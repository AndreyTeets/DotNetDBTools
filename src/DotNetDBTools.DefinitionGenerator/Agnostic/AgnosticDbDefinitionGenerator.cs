using System.Collections.Generic;
using DotNetDBTools.DefinitionGenerator.Shared;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.DefinitionGenerator.Agnostic
{
    public static class AgnosticDefinitionGenerator
    {
        public static IEnumerable<DefinitionSourceFile> GenerateDefinition(AgnosticDatabaseInfo databaseInfo)
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
