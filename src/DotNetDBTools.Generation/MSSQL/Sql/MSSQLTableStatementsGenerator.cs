using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Generation.MSSQL.Sql;

internal class MSSQLTableStatementsGenerator : TableStatementsGenerator<MSSQLTable, MSSQLTableDiff>
{
    protected override string GetCreateSqlImpl(MSSQLTable table)
    {
        string res =
$@"{GetIdDeclarationText(table, 0)}CREATE TABLE [{table.Name}]
(
{GetTableDefinitionsText(table)}
);";

        return res;
    }

    protected override string GetDropSqlImpl(MSSQLTable table)
    {
        return $"DROP TABLE [{table.Name}];";
    }

    protected override string GetAlterSqlImpl(MSSQLTableDiff tableDiff)
    {
        StringBuilder sb = new();

        if (tableDiff.NewTableName != tableDiff.OldTableName)
            sb.AppendLine(Statements.RenameTable(tableDiff.OldTableName, tableDiff.NewTableName));

        foreach (ColumnDiff columnDiff in tableDiff.ColumnsToAlter.Where(x => x.NewColumnName != x.OldColumnName))
            sb.AppendLine(Statements.RenameColumn(tableDiff.NewTableName, columnDiff.OldColumnName, columnDiff.NewColumnName));

        string tableAlters = GetTableAltersText(tableDiff);
        if (!string.IsNullOrEmpty(tableAlters))
            sb.AppendLine(tableAlters);

        return sb.ToString();
    }

    private string GetTableDefinitionsText(Table table)
    {
        List<string> tableDefinitions = new();

        tableDefinitions.AddRange(table.Columns.Select(c =>
$@"    {GetIdDeclarationText(c, 4)}{Statements.DefColumn(c)}"));

        if (table.PrimaryKey is not null)
        {
            tableDefinitions.Add(
$@"    {GetIdDeclarationText(table.PrimaryKey, 4)}{Statements.DefPrimaryKey(table.PrimaryKey)}");
        }

        tableDefinitions.AddRange(table.UniqueConstraints.Select(uc =>
$@"    {GetIdDeclarationText(uc, 4)}{Statements.DefUniqueConstraint(uc)}"));

        tableDefinitions.AddRange(table.CheckConstraints.Select(ck =>
$@"    {GetIdDeclarationText(ck, 4)}{Statements.DefCheckConstraint(ck)}"));

        tableDefinitions.AddRange(table.ForeignKeys.Select(fk =>
$@"    {GetIdDeclarationText(fk, 4)}{Statements.DefForeignKey(fk)}"));

        return string.Join(",\n", tableDefinitions);
    }

    private string GetTableAltersText(TableDiff tableDiff)
    {
        StringBuilder sb = new();

        foreach (ForeignKey fk in tableDiff.ForeignKeysToDrop)
            sb.Append(Statements.DropForeignKey(fk));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToDrop)
            sb.Append(Statements.DropCheckConstraint(tableDiff.NewTableName, ck));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
            sb.Append(Statements.DropUniqueConstraint(tableDiff.NewTableName, uc));
        if (tableDiff.PrimaryKeyToDrop is not null)
            sb.Append(Statements.DropPrimaryKey(tableDiff.NewTableName, tableDiff.PrimaryKeyToDrop));

        AppendColumnsAlters(sb, tableDiff);

        if (tableDiff.PrimaryKeyToCreate is not null)
            sb.Append(Statements.AddPrimaryKey(tableDiff.NewTableName, tableDiff.PrimaryKeyToCreate));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
            sb.Append(Statements.AddUniqueConstraint(tableDiff.NewTableName, uc));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
            sb.Append(Statements.AddCheckConstraint(tableDiff.NewTableName, ck));
        foreach (ForeignKey fk in tableDiff.ForeignKeysToCreate)
            sb.Append(Statements.AddForeignKey(fk));

        return sb.ToString();
    }

    private void AppendColumnsAlters(StringBuilder sb, TableDiff tableDiff)
    {
        foreach (Column column in tableDiff.ColumnsToDrop)
        {
            if (column.Default is not null)
                sb.Append(Statements.DropDefaultConstraint(tableDiff.NewTableName, ((MSSQLColumn)column).DefaultConstraintName));
            sb.Append(Statements.DropColumn(tableDiff.NewTableName, column));
        }

        foreach (ColumnDiff columnDiff in tableDiff.ColumnsToAlter)
        {
            MSSQLColumnDiff cDiff = (MSSQLColumnDiff)columnDiff;
            if (cDiff.DefaultToDrop is not null)
                sb.Append(Statements.DropDefaultConstraint(tableDiff.NewTableName, cDiff.DefaultToDropConstraintName));

            if (cDiff.DataTypeToSet is not null || cDiff.NotNullToSet is not null)
            {
                if (cDiff.DataTypeToSet is null || cDiff.NotNullToSet is null)
                    throw new Exception($"Invalid columnDiff with only one of [DataType|NotNull]ToSet specified");
                sb.Append(Statements.AlterColumnTypeAndNullability(
                    tableDiff.NewTableName, cDiff.NewColumnName, cDiff.DataTypeToSet, cDiff.NotNullToSet.Value));
            }

            if (cDiff.DefaultToSet is not null)
            {
                sb.Append(Statements.AddDefaultConstraint(
                    tableDiff.NewTableName, cDiff.NewColumnName, cDiff.DefaultToSet, cDiff.DefaultToSetConstraintName));
            }
        }

        foreach (Column column in tableDiff.ColumnsToAdd)
            sb.Append(Statements.AddColumn(tableDiff.NewTableName, column));
    }

    private static class Statements
    {
        public static string DefColumn(Column c) =>
$@"[{c.Name}] {c.DataType.Name}{Identity(c)} {Nullability(c.NotNull)}{Default(c.Default, ((MSSQLColumn)c).DefaultConstraintName)}"
            ;
        public static string DefPrimaryKey(PrimaryKey pk) =>
$@"CONSTRAINT [{pk.Name}] PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"[{x}]"))})"
            ;
        public static string DefUniqueConstraint(UniqueConstraint uc) =>
$@"CONSTRAINT [{uc.Name}] UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"[{x}]"))})"
            ;
        public static string DefCheckConstraint(CheckConstraint ck) =>
$@"CONSTRAINT [{ck.Name}] CHECK ({ck.GetExpression()})"
            ;
        public static string DefForeignKey(ForeignKey fk) =>
$@"CONSTRAINT [{fk.Name}] FOREIGN KEY ({string.Join(", ", fk.ThisColumnNames.Select(x => $@"[{x}]"))})
        REFERENCES [{fk.ReferencedTableName}] ({string.Join(", ", fk.ReferencedTableColumnNames.Select(x => $@"[{x}]"))})
        ON UPDATE {fk.OnUpdate} ON DELETE {fk.OnDelete}"
            ;

        public static string RenameTable(string oldTableName, string newTableName) =>
$@"EXEC sp_rename '{oldTableName}', '{newTableName}';"
            ;
        public static string RenameColumn(string tableName, string oldColumnName, string newColumnName) =>
$@"
EXEC sp_rename '{tableName}.{oldColumnName}', '{newColumnName}', 'COLUMN';"
            ;

        public static string AddColumn(string tableName, Column c) =>
$@"
ALTER TABLE [{tableName}] ADD {DefColumn(c)}{WithValues(c)};"
            ;
        public static string DropColumn(string tableName, Column c) =>
$@"
ALTER TABLE [{tableName}] DROP COLUMN [{c.Name}];"
            ;
        public static string AlterColumnTypeAndNullability(string tableName, string cName, DataType dataType, bool notNull) =>
$@"
ALTER TABLE [{tableName}] ALTER COLUMN [{cName}] {dataType.Name} {Nullability(notNull)};"
            ;

        public static string AddDefaultConstraint(string tableName, string cName, CodePiece dValue, string defaultConstraintName) =>
$@"
ALTER TABLE [{tableName}] ADD{Default(dValue, defaultConstraintName)} FOR [{cName}];"
            ;
        public static string DropDefaultConstraint(string tableName, string defaultConstraintName) =>
$@"
ALTER TABLE [{tableName}] DROP CONSTRAINT [{defaultConstraintName}];"
            ;

        public static string AddPrimaryKey(string tableName, PrimaryKey pk) =>
$@"
ALTER TABLE [{tableName}] ADD {DefPrimaryKey(pk)};"
            ;
        public static string DropPrimaryKey(string tableName, PrimaryKey pk) =>
$@"
ALTER TABLE [{tableName}] DROP CONSTRAINT [{pk.Name}];"
            ;

        public static string AddUniqueConstraint(string tableName, UniqueConstraint uc) =>
$@"
ALTER TABLE [{tableName}] ADD {DefUniqueConstraint(uc)};"
            ;
        public static string DropUniqueConstraint(string tableName, UniqueConstraint uc) =>
$@"
ALTER TABLE [{tableName}] DROP CONSTRAINT [{uc.Name}];"
            ;

        public static string AddCheckConstraint(string tableName, CheckConstraint ck) =>
$@"
ALTER TABLE [{tableName}] ADD {DefCheckConstraint(ck)};"
            ;
        public static string DropCheckConstraint(string tableName, CheckConstraint ck) =>
$@"
ALTER TABLE [{tableName}] DROP CONSTRAINT [{ck.Name}];"
            ;

        public static string AddForeignKey(ForeignKey fk) =>
$@"
ALTER TABLE [{fk.Parent.Name}] ADD {DefForeignKey(fk)};"
            ;
        public static string DropForeignKey(ForeignKey fk) =>
$@"
ALTER TABLE [{fk.Parent.Name}] DROP CONSTRAINT [{fk.Name}];"
            ;

        private static string Identity(Column c) =>
c.Identity ? " IDENTITY" : ""
            ;
        private static string Nullability(bool notNull) =>
notNull ? "NOT NULL" : "NULL"
            ;
        private static string Default(CodePiece dValue, string defaultConstraintName) =>
dValue is not null ? $@" CONSTRAINT [{defaultConstraintName}] DEFAULT {dValue.Code}" : ""
            ;
        private static string WithValues(Column c) =>
!c.NotNull && c.Default is not null ? " WITH VALUES" : ""
            ;
    }
}
