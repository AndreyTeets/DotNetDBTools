using System.Collections.Generic;
using DotNetDBTools.Models;

namespace DotNetDBTools.Description
{
    public static class TablesDescriptionGenerator
    {
        public static string GenerateTablesDescription(IDatabaseInfo<ITableInfo<IColumnInfo>> databaseInfo)
        {
            List<string> tableInfoDefinitions = new();
            List<string> tableDeclarations = new();
            foreach (ITableInfo<IColumnInfo> table in databaseInfo.Tables)
            {
                string tableInfoDefinition =
$@"        public class {table.Name}Info
        {{
            public readonly string MyColumn1 = nameof(MyColumn1);
            public readonly string MyColumn2 = nameof(MyColumn2);

            public override string ToString() => nameof({table.Name});
            public static implicit operator string({table.Name}Info info) => info.ToString();
        }}";

                string tableDeclaration =
$@"        public static readonly {table.Name}Info {table.Name} = new();";

                tableInfoDefinitions.Add(tableInfoDefinition);
                tableDeclarations.Add(tableDeclaration);
            }

            string res =
$@"namespace SampleDBDescription
{{
    public static class SampleDBTables
    {{
{string.Join("\n", tableInfoDefinitions)}

{string.Join("\n", tableDeclarations)}
    }}
}}";

            return res;
        }
    }
}
