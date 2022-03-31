using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.MSSQL;

internal static class MSSQLDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> GenerateDefinition(Database database, string projectNamespace)
    {
        List<DefinitionSourceFile> res = new();
        res.AddRange(MSSQLFunctionsDefinitionGenerator.Create(database, projectNamespace));
        res.AddRange(MSSQLTypesDefinitionGenerator.Create(database, projectNamespace));
        res.AddRange(TablesDefinitionGenerator.Create(database, projectNamespace));
        res.AddRange(ViewsDefinitionGenerator.Create(database, projectNamespace));
        res.AddRange(CommonDefinitionProjectFilesCreator.Create(projectNamespace));
        return res;
    }
}
