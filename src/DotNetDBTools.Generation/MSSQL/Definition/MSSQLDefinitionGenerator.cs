using System.Collections.Generic;
using DotNetDBTools.Generation.Core.Definition;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Generation.MSSQL.Definition;

internal class MSSQLDefinitionGenerator : DefinitionGenerator<MSSQLTablesCSharpDefinitionGenerator>
{
    protected override void AddDbmsSpecificObjectsFiles(
        List<DefinitionSourceFile> files, Database database, string projectNamespace)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        if (OutputDefinitionKind == OutputDefinitionKind.CSharp)
        {
            files.AddRange(MSSQLTypesCSharpDefinitionGenerator.Create(db, projectNamespace));
            files.AddRange(MSSQLFunctionsCSharpDefinitionGenerator.Create(db, projectNamespace));
        }
        else
        {
            foreach (MSSQLUserDefinedType type in db.UserDefinedTypes)
                AddFile(files, type, "Types");
            foreach (MSSQLFunction function in db.Functions)
                AddFile(files, function, "Functions");
        }
    }
}
