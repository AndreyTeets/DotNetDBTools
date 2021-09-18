﻿using System;
using System.Collections.Generic;
using DotNetDBTools.Models.Common;

namespace DotNetDBTools.Description.Common
{
    public static class TablesDescriptionGenerator
    {
        public static string GenerateTablesDescription(IDatabaseInfo<ITableInfo<IColumnInfo>> databaseInfo)
        {
            if (string.IsNullOrEmpty(databaseInfo.Name))
                throw new InvalidOperationException("Database name is not set when generating description");

            List<string> tableInfoDefinitions = new();
            List<string> tableDeclarations = new();
            foreach (ITableInfo<IColumnInfo> table in databaseInfo.Tables)
            {
                List<string> columnDeclarations = new();
                foreach (IColumnInfo column in table.Columns)
                {
                    string columnDeclaration =
$@"            public readonly string {column.Name} = nameof({column.Name});";
                    columnDeclarations.Add(columnDeclaration);
                }

                string tableInfoDefinition =
$@"        public class {table.Name}Info
        {{
{string.Join("\n", columnDeclarations)}

            public override string ToString() => nameof({table.Name});
            public static implicit operator string({table.Name}Info info) => info.ToString();
        }}";

                string tableDeclaration =
$@"        public static readonly {table.Name}Info {table.Name} = new();";

                tableInfoDefinitions.Add(tableInfoDefinition);
                tableDeclarations.Add(tableDeclaration);
            }

            string res =
$@"namespace {databaseInfo.Name}Description
{{
    public static class {databaseInfo.Name}Tables
    {{
{string.Join("\n", tableInfoDefinitions)}

{string.Join("\n", tableDeclarations)}
    }}
}}";

            return res;
        }
    }
}
