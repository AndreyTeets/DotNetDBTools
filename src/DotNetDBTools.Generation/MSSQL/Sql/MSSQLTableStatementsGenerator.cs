using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Generation.MSSQL;
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
            sb.Append(Statements.DropPrimaryKey(tableDiff.NewTable.Name, tableDiff.PrimaryKeyToDrop));

        AppendColumnsAlters(sb, tableDiff);

        if (tableDiff.PrimaryKeyToCreate is not null)
            sb.Append(Statements.AddPrimaryKey(tableDiff.NewTable.Name, tableDiff.PrimaryKeyToCreate));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
            sb.Append(Statements.AddUniqueConstraint(tableDiff.NewTable.Name, uc));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
            sb.Append(Statements.AddCheckConstraint(tableDiff.NewTable.Name, ck));
        foreach (ForeignKey fk in tableDiff.ForeignKeysToCreate)
            sb.Append(Statements.AddForeignKey(fk));

        return sb.ToString();
    }

    private void AppendColumnsAlters(StringBuilder sb, TableDiff tableDiff)
    {
        foreach (Column column in tableDiff.RemovedColumns)
        {
            if (column.Default.Code is not null)
                sb.Append(Statements.DropDefaultConstraint(tableDiff.NewTable.Name, column));
            sb.Append(Statements.DropColumn(tableDiff.NewTable.Name, column));
        }

        foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
        {
            bool defaultChagned = columnDiff.NewColumn.Default.Code != columnDiff.OldColumn.Default.Code ||
                ((MSSQLColumn)columnDiff.NewColumn).DefaultConstraintName != ((MSSQLColumn)columnDiff.OldColumn).DefaultConstraintName;
            bool typeOrNullabilityChanged = columnDiff.DataTypeChanged ||
                columnDiff.NewColumn.NotNull != columnDiff.OldColumn.NotNull;

            if (columnDiff.OldColumn.Default.Code is not null && (defaultChagned || typeOrNullabilityChanged))
                sb.Append(Statements.DropDefaultConstraint(tableDiff.NewTable.Name, columnDiff.OldColumn));

            if (typeOrNullabilityChanged)
                sb.Append(Statements.AlterColumnTypeAndNullability(tableDiff.NewTable.Name, columnDiff.NewColumn));

            if (columnDiff.NewColumn.Default.Code is not null && (defaultChagned || typeOrNullabilityChanged))
                sb.Append(Statements.AddDefaultConstraint(tableDiff.NewTable.Name, columnDiff.NewColumn));
        }

        foreach (Column column in tableDiff.AddedColumns)
            sb.Append(Statements.AddColumn(tableDiff.NewTable.Name, column));
    }

    private static class Statements
    {
        public static string DefColumn(Column c) =>
$@"[{c.Name}] {c.DataType.Name}{Identity(c)} {Nullability(c)}{Default(c)}"
            ;
        public static string DefPrimaryKey(PrimaryKey pk) =>
$@"CONSTRAINT [{pk.Name}] PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"[{x}]"))})"
            ;
        public static string DefUniqueConstraint(UniqueConstraint uc) =>
$@"CONSTRAINT [{uc.Name}] UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"[{x}]"))})"
            ;
        public static string DefCheckConstraint(CheckConstraint ck) =>
$@"CONSTRAINT [{ck.Name}] CHECK ({ck.GetCode()})"
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
        public static string AlterColumnTypeAndNullability(string tableName, Column c) =>
$@"
ALTER TABLE [{tableName}] ALTER COLUMN [{c.Name}] {c.DataType.Name} {Nullability(c)};"
            ;

        public static string AddDefaultConstraint(string tableName, Column c) =>
$@"
ALTER TABLE [{tableName}] ADD{Default(c)} FOR [{c.Name}];"
            ;
        public static string DropDefaultConstraint(string tableName, Column c) =>
$@"
ALTER TABLE [{tableName}] DROP CONSTRAINT [{((MSSQLColumn)c).DefaultConstraintName}];"
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
ALTER TABLE [{fk.ThisTableName}] ADD {DefForeignKey(fk)};"
            ;
        public static string DropForeignKey(ForeignKey fk) =>
$@"
ALTER TABLE [{fk.ThisTableName}] DROP CONSTRAINT [{fk.Name}];"
            ;

        private static string Identity(Column c) =>
c.Identity ? " IDENTITY" : ""
            ;
        private static string Nullability(Column c) =>
c.NotNull ? "NOT NULL" : "NULL"
            ;
        private static string Default(Column c) =>
c.GetCode() is not null ? $@" CONSTRAINT [{((MSSQLColumn)c).DefaultConstraintName}] DEFAULT {c.GetCode()}" : ""
            ;
        private static string WithValues(Column c) =>
!c.NotNull && c.Default.Code is not null ? " WITH VALUES" : ""
            ;
    }
}
