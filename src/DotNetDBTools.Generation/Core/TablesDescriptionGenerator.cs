using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core;

internal static class TablesDescriptionGenerator
{
    public static string Create(Database database, GenerationOptions options)
    {
        List<string> tableDescriptionDefinitions = new();
        List<string> tableDeclarations = new();
        foreach (Table table in database.Tables)
        {
            List<string> columnDeclarations = new();
            foreach (Column column in table.Columns)
            {
                string columnDeclaration =
$@"            public readonly string {column.Name} = nameof({column.Name});";
                columnDeclarations.Add(columnDeclaration);
            }

            string tableDescriptionDefinition =
$@"        public class {table.Name}Description
        {{
{string.Join("\n", columnDeclarations)}

            public override string ToString() => nameof({table.Name});
            public static implicit operator string({table.Name}Description description) => description.ToString();
        }}";

            string tableDeclaration =
$@"        public static readonly {table.Name}Description {table.Name} = new();";

            tableDescriptionDefinitions.Add(tableDescriptionDefinition);
            tableDeclarations.Add(tableDeclaration);
        }

        string res =
$@"namespace {options.DatabaseName}Description
{{
    public static class {options.DatabaseName}Tables
    {{
{string.Join("\n", tableDeclarations)}

{string.Join("\n", tableDescriptionDefinitions)}
    }}
}}";

        return res.NormalizeLineEndings();
    }
}
