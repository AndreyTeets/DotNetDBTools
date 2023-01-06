using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Generation.Core.Definition.CSharpDefinitionGenerationHelper;

namespace DotNetDBTools.Generation.Core.Definition;

internal static class TablesCSharpDefinitionGenerator
{
    public static IEnumerable<DefinitionSourceFile> Create(Database database, string projectNamespace)
    {
        List<DefinitionSourceFile> res = new();
        foreach (Table table in database.Tables)
        {
            List<DefinitionSourceFile> sqlRefFiles = new();

            List<string> columnDeclarations = new();
            foreach (Column column in table.Columns)
            {
                List<string> propsDeclarations = new();
                propsDeclarations.Add($@"            DataType = {DeclareDataType(column.DataType)},");
                if (column.NotNull)
                    propsDeclarations.Add($@"            NotNull = {DeclareBool(column.NotNull)},");
                if (column.Identity)
                    propsDeclarations.Add($@"            Identity = {DeclareBool(column.Identity)},");
                if (column.Default.Code is not null)
                    propsDeclarations.Add($@"            Default = {DeclareDefaultValue(column.Default)},");

                string columnDeclaration =
$@"        public Column {column.Name} = new(""{column.ID}"")
        {{
{string.Join("\n", propsDeclarations)}
        }};";

                columnDeclarations.Add(columnDeclaration);
            }

            string pkDeclaration = null;
            if (table.PrimaryKey is not null)
            {
                pkDeclaration =
$@"        public PrimaryKey {table.PrimaryKey.Name} = new(""{table.PrimaryKey.ID}"")
        {{
{CreateColumnsDeclaration("Columns", table.PrimaryKey.Columns)}
        }};";
            }

            List<string> ucDeclarations = new();
            foreach (UniqueConstraint uc in table.UniqueConstraints)
            {
                string ucDeclaration =
$@"        public UniqueConstraint {uc.Name} = new(""{uc.ID}"")
        {{
{CreateColumnsDeclaration("Columns", uc.Columns)}
        }};";

                ucDeclarations.Add(ucDeclaration);
            }

            List<string> fkDeclarations = new();
            foreach (ForeignKey fk in table.ForeignKeys)
            {
                string fkDeclaration =
$@"        public ForeignKey {fk.Name} = new(""{fk.ID}"")
        {{
{CreateColumnsDeclaration("ThisColumns", fk.ThisColumnNames)}
            ReferencedTable = ""{fk.ReferencedTableName}"",
{CreateColumnsDeclaration("ReferencedTableColumns", fk.ReferencedTableColumnNames)}
            OnUpdate = ForeignKeyActions.{MapActionName(fk.OnUpdate)},
            OnDelete = ForeignKeyActions.{MapActionName(fk.OnDelete)},
        }};";

                fkDeclarations.Add(fkDeclaration);
            }

            List<string> idxDeclarations = new();
            foreach (Index idx in table.Indexes)
            {
                List<string> propsDeclarations = new();
                propsDeclarations.Add(CreateColumnsDeclaration("Columns", idx.Columns));
                if (idx.Unique)
                    propsDeclarations.Add($@"            Unique = {DeclareBool(idx.Unique)},");

                string idxDeclaration =
$@"        public Index {idx.Name} = new(""{idx.ID}"")
        {{
{string.Join("\n", propsDeclarations)}
        }};";

                idxDeclarations.Add(idxDeclaration);
            }

            List<string> trDeclarations = new();
            foreach (Trigger tr in table.Triggers)
            {
                string trDeclaration =
$@"        public Trigger {tr.Name} = new(""{tr.ID}"")
        {{
            Code = ""Triggers.{tr.Name}.sql"".AsSqlResource(),
        }};";

                trDeclarations.Add(trDeclaration);
                DefinitionSourceFile sqlRefFile = new()
                {
                    RelativePath = $"Sql/Triggers/{tr.Name}.sql",
                    SourceText = tr.CodePiece.Code.NormalizeLineEndings(),
                };
                sqlRefFiles.Add(sqlRefFile);
            }

            List<string> ckDeclarations = new();
            foreach (CheckConstraint ck in table.CheckConstraints)
            {
                string ckDeclaration =
$@"        public CheckConstraint {ck.Name} = new(""{ck.ID}"")
        {{
            Code = {DeclareString(ck.CodePiece.Code)},
        }};";

                ckDeclarations.Add(ckDeclaration);
            }

            List<string> allDeclarations = new();
            allDeclarations.AddRange(columnDeclarations);
            if (!string.IsNullOrEmpty(pkDeclaration))
                allDeclarations.Add(pkDeclaration);
            allDeclarations.AddRange(ucDeclarations);
            allDeclarations.AddRange(fkDeclarations);
            allDeclarations.AddRange(idxDeclarations);
            allDeclarations.AddRange(trDeclarations);
            allDeclarations.AddRange(ckDeclarations);

            string tableCode =
$@"using System;
using DotNetDBTools.Definition.{database.Kind};

namespace {projectNamespace}.Tables
{{
    public class {table.Name} : ITable
    {{
        public Guid DNDBT_OBJECT_ID => new(""{table.ID}"");

{string.Join("\n\n", allDeclarations)}
    }}
}}";

            DefinitionSourceFile file = new()
            {
                RelativePath = $"Tables/{table.Name}.cs",
                SourceText = tableCode.NormalizeLineEndings(),
            };
            res.Add(file);
            res.AddRange(sqlRefFiles);
        }
        return res;
    }

    private static string CreateColumnsDeclaration(string propName, IEnumerable<string> columns)
    {
        return $@"            {propName} = new[] {{ {string.Join(", ", columns.Select(x => $"\"{x}\""))} }},";
    }

    private static string MapActionName(string actionName)
    {
        return actionName switch
        {
            ForeignKeyActions.NoAction => nameof(ForeignKeyActions.NoAction),
            ForeignKeyActions.Restrict => nameof(ForeignKeyActions.Restrict),
            ForeignKeyActions.Cascade => nameof(ForeignKeyActions.Cascade),
            ForeignKeyActions.SetDefault => nameof(ForeignKeyActions.SetDefault),
            ForeignKeyActions.SetNull => nameof(ForeignKeyActions.SetNull),
            _ => throw new InvalidOperationException($"Invalid actionName: '{actionName}'")
        };
    }
}
