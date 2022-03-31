using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.PostgreSQL;

internal static class PostgreSQLDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> GenerateDefinition(Database database, string projectNamespace)
    {
        List<DefinitionSourceFile> res = new();
        res.AddRange(PostgreSQLFunctionsDefinitionGenerator.Create(database, projectNamespace));
        res.AddRange(PostgreSQLTypesDefinitionGenerator.Create(database, projectNamespace));
        res.AddRange(TablesDefinitionGenerator.Create(database, projectNamespace));
        res.AddRange(ViewsDefinitionGenerator.Create(database, projectNamespace));
        res.AddRange(CommonDefinitionProjectFilesCreator.Create(projectNamespace));
        return res;
    }
}
