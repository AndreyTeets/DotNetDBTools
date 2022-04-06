using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;
using static DotNetDBTools.Generation.Core.DefinitionGenerationHelper;

namespace DotNetDBTools.Generation.MSSQL;

internal static class MSSQLTypesDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> Create(Database database, string projectNamespace)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        List<DefinitionSourceFile> res = new();

        foreach (MSSQLUserDefinedType type in db.UserDefinedTypes)
        {
            string typeCode =
$@"using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.{db.Kind};
using DotNetDBTools.Definition.{db.Kind}.UserDefinedTypes;

namespace {projectNamespace}.Types
{{
    public class {type.Name} : IUserDefinedType
    {{
        public Guid ID => new(""{type.ID}"");
        public IDataType UnderlyingType => {DeclareDataType(type.UnderlyingType)};
        public bool NotNull => {DeclareBool(type.NotNull)};
    }}
}}";

            DefinitionSourceFile file = new()
            {
                RelativePath = $"Types/{type.Name}.cs",
                SourceText = typeCode.NormalizeLineEndings(),
            };
            res.Add(file);
        }

        foreach (MSSQLUserDefinedTableType type in db.UserDefinedTableTypes)
        {
            List<string> ucDeclarations = new();
            foreach (UniqueConstraint uc in type.UniqueConstraints)
            {
                string ckDeclaration =
$@"        public UniqueConstraint {uc.Name} = new(""{uc.ID}"")
        {{
        }};";

                ucDeclarations.Add(ckDeclaration);
            }

            string typeCode =
$@"using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.{db.Kind};
using DotNetDBTools.Definition.{db.Kind}.UserDefinedTypes;

namespace {projectNamespace}.TableTypes
{{
    public class {type.Name} : IUserDefinedTableType
    {{
        public Guid ID => new(""{type.ID}"");

{string.Join("\n\n", ucDeclarations)}
    }}
}}";

            DefinitionSourceFile file = new()
            {
                RelativePath = $"TableTypes/{type.Name}.cs",
                SourceText = typeCode.NormalizeLineEndings(),
            };
            //res.Add(file);
        }

        return res;
    }
}
