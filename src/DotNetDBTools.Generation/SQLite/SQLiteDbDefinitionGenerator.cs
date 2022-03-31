using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.SQLite;

internal static class SQLiteDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> GenerateDefinition(Database database, string projectNamespace)
    {
        List<DefinitionSourceFile> res = new();
        res.AddRange(TablesDefinitionGenerator.Create(database, projectNamespace));
        res.AddRange(ViewsDefinitionGenerator.Create(database, projectNamespace));
        res.AddRange(CommonDefinitionProjectFilesCreator.Create(projectNamespace));
        return res;
    }
}
