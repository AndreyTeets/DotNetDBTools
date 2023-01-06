using System.Collections.Generic;
using DotNetDBTools.Generation.Core.Definition;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Generation.MySQL.Definition;

internal class MySQLDefinitionGenerator : DefinitionGenerator
{
    protected override void AddDbmsSpecificObjectsFiles(
        List<DefinitionSourceFile> files, Database database, string projectNamespace)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        if (OutputDefinitionKind == OutputDefinitionKind.CSharp)
        {
            files.AddRange(MySQLFunctionsCSharpDefinitionGenerator.Create(db, projectNamespace));
        }
        else
        {
            foreach (MySQLFunction function in db.Functions)
                AddFile(files, function, "Functions");
        }
    }
}

