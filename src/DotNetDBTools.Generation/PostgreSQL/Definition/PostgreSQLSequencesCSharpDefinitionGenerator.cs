using System.Collections.Generic;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using static DotNetDBTools.Generation.Core.Definition.CSharpDefinitionGenerationHelper;

namespace DotNetDBTools.Generation.PostgreSQL.Definition;

internal static class PostgreSQLSequencesCSharpDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> Create(Database database, string projectNamespace)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        List<DefinitionSourceFile> res = new();
        foreach (PostgreSQLSequence sequence in db.Sequences)
        {
            string sequenceCode =
$@"using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.{database.Kind};

namespace {projectNamespace}.Sequences
{{
    public class {sequence.Name} : ISequence
    {{
        public Guid DNDBT_OBJECT_ID => new(""{sequence.ID}"");
        public IDataType DataType => {DeclareDataType(sequence.DataType)};
        public SequenceOptions Options => new()
        {{
            StartWith = {sequence.Options.StartWith},
            IncrementBy = {sequence.Options.IncrementBy},
            MinValue = {sequence.Options.MinValue},
            MaxValue = {sequence.Options.MaxValue},
            Cache = {sequence.Options.Cache},
            Cycle = {DeclareBool(sequence.Options.Cycle.Value)},
        }};
        public (string TableName, string ColumnName) OwnedBy => ({DeclareString(sequence.OwnedBy.TableName)}, {DeclareString(sequence.OwnedBy.ColumnName)});
    }}
}}";

            DefinitionSourceFile file = new()
            {
                RelativePath = $"Sequences/{sequence.Name}.cs",
                SourceText = sequenceCode.NormalizeLineEndings(),
            };
            res.Add(file);
        }
        return res;
    }
}
