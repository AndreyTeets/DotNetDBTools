using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Definition;

internal static class PostgreSQLProceduresCSharpDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> Create(Database database, string projectNamespace)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        List<DefinitionSourceFile> res = new();
        foreach (PostgreSQLProcedure proc in db.Procedures)
        {
            string procCode =
$@"using System;
using DotNetDBTools.Definition.{database.Kind};

namespace {projectNamespace}.Procedures
{{
    public class {proc.Name} : IProcedure
    {{
        public Guid DNDBT_OBJECT_ID => new(""{proc.ID}"");
        public string CreateStatement => ""Procedures.{proc.Name}.sql"".AsSqlResource();
    }}
}}";

            DefinitionSourceFile file = new()
            {
                RelativePath = $"Procedures/{proc.Name}.cs",
                SourceText = procCode.NormalizeLineEndings(),
            };
            DefinitionSourceFile sqlRefFile = new()
            {
                RelativePath = $"Sql/Procedures/{proc.Name}.sql",
                SourceText = proc.GetCreateStatement().NormalizeLineEndings(),
            };
            res.Add(file);
            res.Add(sqlRefFile);
        }
        return res;
    }
}
