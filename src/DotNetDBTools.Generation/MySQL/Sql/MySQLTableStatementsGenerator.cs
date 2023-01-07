﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Generation.MySQL.Sql;

internal class MySQLTableStatementsGenerator : TableStatementsGenerator<MySQLTable, MySQLTableDiff>
{
    protected override string GetCreateSqlImpl(MySQLTable table)
    {
        string res =
$@"{GetIdDeclarationText(table, 0)}CREATE TABLE `{table.Name}`
(
{GetTableDefinitionsText(table)}
);";

        return res;
    }

    protected override string GetDropSqlImpl(MySQLTable table)
    {
        return $"DROP TABLE `{table.Name}`;";
    }

    protected override string GetAlterSqlImpl(MySQLTableDiff tableDiff)
    {
        StringBuilder sb = new();

        if (tableDiff.NewTable.Name != tableDiff.OldTable.Name)
            sb.AppendLine(Statements.RenameTable(tableDiff.OldTable.Name, tableDiff.NewTable.Name));

        foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns.Where(x => x.NewColumn.Name != x.OldColumn.Name))
            sb.AppendLine(Statements.RenameColumn(tableDiff.NewTable.Name, columnDiff.OldColumn.Name, columnDiff.NewColumn.Name));

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
            sb.Append(Statements.DropCheckConstraint(tableDiff.NewTable.Name, ck));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
            sb.Append(Statements.DropUniqueConstraint(tableDiff.NewTable.Name, uc));

        if (tableDiff.PrimaryKeyToDrop is not null)
        {
            Column identityColumn = tableDiff.OldTable.Columns.SingleOrDefault(c => c.Identity);
            if (tableDiff.PrimaryKeyToDrop.Columns.Any(c => c == identityColumn?.Name))
                sb.Append(Statements.DropPrimaryKeyAndColumnIdentityAttribute(tableDiff.NewTable.Name, identityColumn));
            else
                sb.Append(Statements.DropPrimaryKey(tableDiff.NewTable.Name));
        }
        bool addedPk = AppendColumnsAlters(sb, tableDiff);
        if (tableDiff.PrimaryKeyToCreate is not null && !addedPk)
            sb.Append(Statements.AddPrimaryKey(tableDiff.NewTable.Name, tableDiff.PrimaryKeyToCreate));

        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
            sb.Append(Statements.AddUniqueConstraint(tableDiff.NewTable.Name, uc));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
            sb.Append(Statements.AddCheckConstraint(tableDiff.NewTable.Name, ck));
        foreach (ForeignKey fk in tableDiff.ForeignKeysToCreate)
            sb.Append(Statements.AddForeignKey(fk));

        return sb.ToString();
    }

    private bool AppendColumnsAlters(StringBuilder sb, TableDiff tableDiff)
    {
        foreach (Column column in tableDiff.RemovedColumns)
            sb.Append(Statements.DropColumn(tableDiff.NewTable.Name, column));

        foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
        {
            bool defaultChagned = columnDiff.NewColumn.Default.Code != columnDiff.OldColumn.Default.Code;
            if (columnDiff.DataTypeChanged || columnDiff.NewColumn.NotNull != columnDiff.OldColumn.NotNull)
                sb.Append(Statements.AlterColumnDefinition(tableDiff.NewTable.Name, columnDiff.NewColumn));
            else if (columnDiff.NewColumn.Default.Code is not null && defaultChagned)
                sb.Append(Statements.AddDefaultConstraint(tableDiff.NewTable.Name, columnDiff.NewColumn));
            else if (columnDiff.NewColumn.Default.Code is null && defaultChagned)
                sb.Append(Statements.DropDefaultConstraint(tableDiff.NewTable.Name, columnDiff.NewColumn));
        }

        bool addedPk = false;
        foreach (Column column in tableDiff.AddedColumns)
        {
            if (tableDiff.PrimaryKeyToCreate is not null && tableDiff.PrimaryKeyToCreate.Columns.Any(c => c == column.Name))
            {
                sb.Append(Statements.AddColumnAsPrimaryKey(tableDiff.NewTable.Name, column, tableDiff.PrimaryKeyToCreate));
                addedPk = true;
            }
            else
            {
                sb.Append(Statements.AddColumn(tableDiff.NewTable.Name, column));
            }
        }
        return addedPk;
    }

    private static class Statements
    {
        public static string DefColumn(Column c) =>
$@"`{c.Name}` {c.DataType.Name}{Identity(c)} {Nullability(c)}{Default(c)}"
            ;
        public static string DefPrimaryKey(PrimaryKey pk) =>
$@"CONSTRAINT `{pk.Name}` PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"`{x}`"))})"
            ;
        public static string DefUniqueConstraint(UniqueConstraint uc) =>
$@"CONSTRAINT `{uc.Name}` UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"`{x}`"))})"
            ;
        public static string DefCheckConstraint(CheckConstraint ck) =>
$@"CONSTRAINT `{ck.Name}` CHECK ({ck.GetCode()})"
            ;
        public static string DefForeignKey(ForeignKey fk) =>
$@"CONSTRAINT `{fk.Name}` FOREIGN KEY ({string.Join(", ", fk.ThisColumnNames.Select(x => $@"`{x}`"))})
        REFERENCES `{fk.ReferencedTableName}` ({string.Join(", ", fk.ReferencedTableColumnNames.Select(x => $@"`{x}`"))})
        ON UPDATE {fk.OnUpdate} ON DELETE {fk.OnDelete}"
            ;

        public static string RenameTable(string oldTableName, string newTableName) =>
$@"RENAME TABLE `{oldTableName}` TO `{newTableName}`;"
            ;
        public static string RenameColumn(string tableName, string oldColumnName, string newColumnName) =>
$@"
ALTER TABLE `{tableName}` RENAME COLUMN `{oldColumnName}` TO `{newColumnName}`;"
            ;

        public static string AddColumn(string tableName, Column c) =>
$@"
ALTER TABLE `{tableName}` ADD COLUMN {DefColumn(c)};"
            ;
        public static string AddColumnAsPrimaryKey(string tableName, Column c, PrimaryKey pk) =>
$@"
ALTER TABLE `{tableName}` ADD COLUMN `{c.Name}` {c.DataType.Name}{Identity(c)} {Nullability(c)},
     ADD PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"`{x}`"))});"
;
        public static string DropColumn(string tableName, Column c) =>
$@"
ALTER TABLE `{tableName}` DROP COLUMN `{c.Name}`;"
            ;
        public static string AlterColumnDefinition(string tableName, Column c) =>
$@"
ALTER TABLE `{tableName}` MODIFY COLUMN `{c.Name}` {c.DataType.Name} {Nullability(c)}{Default(c)};"
            ;

        public static string AddDefaultConstraint(string tableName, Column c) =>
$@"
ALTER TABLE `{tableName}` ALTER COLUMN `{c.Name}` SET DEFAULT {c.GetCode()};"
            ;
        public static string DropDefaultConstraint(string tableName, Column c) =>
$@"
ALTER TABLE `{tableName}` ALTER COLUMN `{c.Name}` DROP DEFAULT;"
            ;

        public static string AddPrimaryKey(string tableName, PrimaryKey pk) =>
$@"
ALTER TABLE `{tableName}` ADD PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"`{x}`"))});"
            ;
        public static string DropPrimaryKey(string tableName) =>
$@"
ALTER TABLE `{tableName}` DROP PRIMARY KEY;"
;
        public static string DropPrimaryKeyAndColumnIdentityAttribute(string tableName, Column identityColumn) =>
$@"
ALTER TABLE `{tableName}` DROP PRIMARY KEY,
    MODIFY COLUMN `{identityColumn.Name}` {identityColumn.DataType.Name} {Nullability(identityColumn)};"
            ;

        public static string AddUniqueConstraint(string tableName, UniqueConstraint uc) =>
$@"
ALTER TABLE `{tableName}` ADD {DefUniqueConstraint(uc)};"
            ;
        public static string DropUniqueConstraint(string tableName, UniqueConstraint uc) =>
$@"
ALTER TABLE `{tableName}` DROP CONSTRAINT `{uc.Name}`;"
            ;

        public static string AddCheckConstraint(string tableName, CheckConstraint ck) =>
$@"
ALTER TABLE `{tableName}` ADD {DefCheckConstraint(ck)};"
            ;
        public static string DropCheckConstraint(string tableName, CheckConstraint ck) =>
$@"
ALTER TABLE `{tableName}` DROP CONSTRAINT `{ck.Name}`;"
            ;

        public static string AddForeignKey(ForeignKey fk) =>
$@"
ALTER TABLE `{fk.ThisTableName}` ADD {DefForeignKey(fk)};"
            ;
        public static string DropForeignKey(ForeignKey fk) =>
$@"
ALTER TABLE `{fk.ThisTableName}` DROP CONSTRAINT `{fk.Name}`;"
            ;

        private static string Identity(Column c) =>
c.Identity ? " AUTO_INCREMENT" : ""
            ;
        private static string Nullability(Column c) =>
c.NotNull ? "NOT NULL" : "NULL"
            ;
        private static string Default(Column c) =>
c.GetCode() is not null ? $@" DEFAULT {c.GetCode()}" : ""
            ;
    }
}