﻿using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Definition;

internal static class PostgreSQLFunctionsCSharpDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> Create(Database database, string projectNamespace)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        List<DefinitionSourceFile> res = new();
        foreach (PostgreSQLFunction func in db.Functions)
        {
            string funcCode =
$@"using System;
using DotNetDBTools.Definition.{database.Kind};

namespace {projectNamespace}.Functions
{{
    public class {func.Name} : IFunction
    {{
        public Guid DNDBT_OBJECT_ID => new(""{func.ID}"");
        public string CreateStatement => ""Functions.{func.Name}.sql"".AsSqlResource();
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
