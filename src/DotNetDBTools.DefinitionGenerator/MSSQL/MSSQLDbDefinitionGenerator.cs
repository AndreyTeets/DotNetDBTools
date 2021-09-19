using System.Collections.Generic;
using DotNetDBTools.DefinitionGenerator.Shared;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.DefinitionGenerator.MSSQL
{
    public static class MSSQLDefinitionGenerator
    {
        public static IEnumerable<DefinitionSourceFile> GenerateDefinition(MSSQLDatabaseInfo databaseInfo)
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
