using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Generation.MySQL.Definition;

internal static class MySQLFunctionsCSharpDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> Create(Database database, string projectNamespace)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        List<DefinitionSourceFile> res = new();
        foreach (MySQLFunction func in db.Functions)
        {
            string funcCode =
$@"using System;
using DotNetDBTools.Definition.{database.Kind};

namespace {projectNamespace}.Functions
{{
    public class {func.Name} : IFunction
    {{
        public Guid DNDBT_OBJECT_ID => new(""{func.ID}"");
        public string Code => ""Functions.{func.Name}.sql"".AsSqlResource();
    }}
}}";

            DefinitionSourceFile file = new()
            {
                RelativePath = $"Functions/{func.Name}.cs",
                SourceText = funcCode.NormalizeLineEndings(),
            };
            DefinitionSourceFile sqlRefFile = new()
            {
                RelativePath = $"Sql/Functions/{func.Name}.sql",
                SourceText = func.GetCreateStatement().NormalizeLineEndings(),
            };
            res.Add(file);
            res.Add(sqlRefFile);
        }
        return res;
    }
}
