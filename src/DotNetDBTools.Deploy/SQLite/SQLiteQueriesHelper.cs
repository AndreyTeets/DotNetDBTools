using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite;

internal static class SQLiteQueriesHelper
{
    public static string GetTableDefinitionsText(Table table)
    {
        List<string> tableDefinitions = new();

        tableDefinitions.AddRange(table.Columns.Select(c =>
$@"    {c.Name} {c.DataType.Name}{GetPrimaryKeyStatement(c, table.PrimaryKey)} {GetNullabilityStatement(c)}{GetDefaultValStatement(c)}"));

        if (table.PrimaryKey is not null && table.PrimaryKey.Columns.Count() > 1)
        {
            tableDefinitions.Add(
$@"    CONSTRAINT {table.PrimaryKey.Name} PRIMARY KEY ({string.Join(", ", table.PrimaryKey.Columns)})");
        }

        tableDefinitions.AddRange(table.UniqueConstraints.Select(uc =>
$@"    CONSTRAINT {uc.Name} UNIQUE ({string.Join(", ", uc.Columns)})"));

        tableDefinitions.AddRange(table.ForeignKeys.Select(fk =>
$@"    CONSTRAINT {fk.Name} FOREIGN KEY ({string.Join(", ", fk.ThisColumnNames)})
        REFERENCES {fk.ReferencedTableName}({string.Join(", ", fk.ReferencedTableColumnNames)})
        ON UPDATE {fk.OnUpdate} ON DELETE {fk.OnDelete}"));

        tableDefinitions.AddRange(table.CheckConstraints.Select(ck =>
$@"    CONSTRAINT [{ck.Name}] {ck.GetCode()}"));

        return string.Join(",\n", tableDefinitions);
    }

    public static string GetUniqueStatement(bool unique)
    {
        if (unique)
            return " UNIQUE";
        else
            return "";
    }

    private static string GetPrimaryKeyStatement(Column column, PrimaryKey pk)
    {
        string identityStatement = column.Identity ? " AUTOINCREMENT" : "";
        if (pk is not null &&
            pk.Columns.Count() == 1 &&
            pk.Columns.Single() == column.Name)
        {
            return $" PRIMARY KEY{identityStatement}";
        }
        return "";
    }

    private static string GetNullabilityStatement(Column column)
    {
        return column.NotNull switch
        {
            false => "NULL",
            true => "NOT NULL",
        };
    }

    private static string GetDefaultValStatement(Column column)
    {
        if (column.Default is not null)
        {
            return $" DEFAULT {QuoteDefaultValue(column.Default)}";
        }
        return "";
    }

    private static string QuoteDefaultValue(object value)
    {
        return value switch
        {
            CodePiece => $"({((CodePiece)value).Code})",
            string => $"'{value}'",
            long => $"{value}",
            decimal val => $"{val.ToString(CultureInfo.InvariantCulture)}",
            byte[] => $"{ToHex((byte[])value)}",
            _ => throw new InvalidOperationException($"Invalid value type: '{value.GetType()}'")
        };

        static string ToHex(byte[] val) => $@"0x{BitConverter.ToString(val).Replace("-", "")}";
    }
}
