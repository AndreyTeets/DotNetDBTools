using System.Collections.Generic;
using DotNetDBTools.Generation.Core.Definition;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.SQLite.Definition;

internal class SQLiteDefinitionGenerator : DefinitionGenerator
{
    protected override void AddDbmsSpecificObjectsFiles(
        List<DefinitionSourceFile> files, Database database, string projectNamespace)
    {
    }
}
