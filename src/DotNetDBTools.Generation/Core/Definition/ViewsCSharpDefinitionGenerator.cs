﻿using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core.Definition;

internal static class ViewsCSharpDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> Create(Database database, string projectNamespace)
    {
        List<DefinitionSourceFile> res = new();
        foreach (View view in database.Views)
        {
            string viewCode =
$@"using System;
using DotNetDBTools.Definition.{database.Kind};

namespace {projectNamespace}.Views
{{
    public class {view.Name} : IView
    {{
        public Guid DNDBT_OBJECT_ID => new(""{view.ID}"");
        public string CreateStatement => ""Views.{view.Name}.sql"".AsSqlResource();
    }}
}}";

            DefinitionSourceFile file = new()
            {
                RelativePath = $"Views/{view.Name}.cs",
                SourceText = viewCode.NormalizeLineEndings(),
            };
            DefinitionSourceFile sqlRefFile = new()
            {
                RelativePath = $"Sql/Views/{view.Name}.sql",
                SourceText = view.GetCreateStatement().NormalizeLineEndings(),
            };
            res.Add(file);
            res.Add(sqlRefFile);
        }
        return res;
    }
}
