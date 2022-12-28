using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;
using static DotNetDBTools.Generation.Core.DefinitionGenerationHelper;

namespace DotNetDBTools.Generation.PostgreSQL;

internal static class PostgreSQLTypesDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> Create(Database database, string projectNamespace)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        List<DefinitionSourceFile> res = new();

        foreach (PostgreSQLCompositeType type in db.CompositeTypes)
        {
            List<string> attributesDeclarations = new();
            foreach (PostgreSQLCompositeTypeAttribute attribute in type.Attributes)
            {
                attributesDeclarations.Add(
$@"            {{ ""{attribute.Name}"", {DeclareDataType(attribute.DataType)} }},");
            }

            string typeCode =
$@"using System;
using System.Collections.Generic;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.{db.Kind};
using DotNetDBTools.Definition.{db.Kind}.UserDefinedTypes;

namespace {projectNamespace}.Types
{{
    public class {type.Name} : ICompositeType
    {{
        public Guid DNDBT_OBJECT_ID => new(""{type.ID}"");
        public IDictionary<string, IDataType> Attributes => new Dictionary<string, IDataType>()
        {{
{string.Join("\n", attributesDeclarations)}
        }};
    }}
}}";

            DefinitionSourceFile file = new()
            {
                RelativePath = $"Types/{type.Name}.cs",
                SourceText = typeCode.NormalizeLineEndings(),
            };
            res.Add(file);
        }

        foreach (PostgreSQLDomainType type in db.DomainTypes)
        {
            List<string> ckDeclarations = new();
            foreach (CheckConstraint ck in type.CheckConstraints)
            {
                string ckDeclaration =
$@"        public CheckConstraint {ck.Name} = new(""{ck.ID}"")
        {{
            Code = {DeclareString(ck.CodePiece.Code)},
        }};";

                ckDeclarations.Add(ckDeclaration);
            }

            string typeCode =
$@"using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.{db.Kind};
using DotNetDBTools.Definition.{db.Kind}.UserDefinedTypes;

namespace {projectNamespace}.Types
{{
    public class {type.Name} : IDomain
    {{
        public Guid DNDBT_OBJECT_ID => new(""{type.ID}"");
        public IDataType UnderlyingType => {DeclareDataType(type.UnderlyingType)};
        public bool NotNull => {type.NotNull.ToString().ToLower()};
        public IDefaultValue Default => {DeclareDefaultValue(type.Default)};

{string.Join("\n\n", ckDeclarations)}
    }}
}}";

            DefinitionSourceFile file = new()
            {
                RelativePath = $"Types/{type.Name}.cs",
                SourceText = typeCode.NormalizeLineEndings(),
            };
            res.Add(file);
        }

        foreach (PostgreSQLEnumType type in db.EnumTypes)
        {
            List<string> allowedValuesDeclarations = new();
            foreach (string value in type.AllowedValues)
                allowedValuesDeclarations.Add($@"            ""{value}"",");

            string typeCode =
$@"using System;
using System.Collections.Generic;
using DotNetDBTools.Definition.{db.Kind}.UserDefinedTypes;

namespace {projectNamespace}.Types
{{
    public class {type.Name} : IEnumType
    {{
        public Guid DNDBT_OBJECT_ID => new(""{type.ID}"");
        public IEnumerable<string> AllowedValues => new[]
        {{
{string.Join("\n", allowedValuesDeclarations)}
        }};
    }}
}}";

            DefinitionSourceFile file = new()
            {
                RelativePath = $"Types/{type.Name}.cs",
                SourceText = typeCode.NormalizeLineEndings(),
            };
            res.Add(file);
        }

        foreach (PostgreSQLRangeType type in db.RangeTypes)
        {
            string typeCode =
$@"using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.{db.Kind};
using DotNetDBTools.Definition.{db.Kind}.UserDefinedTypes;

namespace {projectNamespace}.Types
{{
    public class {type.Name} : IRangeType
    {{
        public Guid DNDBT_OBJECT_ID => new(""{type.ID}"");
        public IDataType Subtype => {DeclareDataType(type.Subtype)};
        public string SubtypeOperatorClass => {DeclareString(type.SubtypeOperatorClass)};
        public string Collation => {DeclareString(type.Collation)};
        public string CanonicalFunction => {DeclareString(type.CanonicalFunction)};
        public string SubtypeDiff => {DeclareString(type.SubtypeDiff)};
        public string MultirangeTypeName => {DeclareString(type.MultirangeTypeName)};
    }}
}}";

            DefinitionSourceFile file = new()
            {
                RelativePath = $"Types/{type.Name}.cs",
                SourceText = typeCode.NormalizeLineEndings(),
            };
            res.Add(file);
        }

        return res;
    }
}
