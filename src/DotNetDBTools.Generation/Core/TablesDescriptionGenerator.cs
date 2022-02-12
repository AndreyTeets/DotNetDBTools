using System;
using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core;

internal static class TablesDescriptionGenerator
{
    public static string GenerateTablesDescription(Database database)
    {
        if (string.IsNullOrEmpty(database.Name))
            throw new InvalidOperationException("Database name is not set when generating description");

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
$@"namespace {database.Name}Description
{{
    public static class {database.Name}Tables
    {{
{string.Join("\n", tableDescriptionDefinitions)}

{string.Join("\n", tableDeclarations)}
    }}
}}";

        return res;
    }
}
