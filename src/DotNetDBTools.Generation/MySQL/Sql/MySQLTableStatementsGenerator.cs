using System.Collections.Generic;
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

        if (tableDiff.NewTableName != tableDiff.OldTableName)
            sb.AppendLine(Statements.RenameTable(tableDiff.OldTableName, tableDiff.NewTableName));

        foreach (ColumnDiff columnDiff in tableDiff.ColumnsToAlter.Where(x => x.NewColumnName != x.OldColumnName))
            sb.AppendLine(Statements.RenameColumn(tableDiff.NewTableName, columnDiff.OldColumnName, columnDiff.NewColumnName));

        string tableAlters = GetTableAltersText(tableDiff);
        if (!string.IsNullOrEmpty(tableAlters))
            sb.AppendLine($@"ALTER TABLE `{tableDiff.NewTableName}`{tableAlters};");

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
            sb.Append(Statements.DropCheckConstraint(ck));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
            sb.Append(Statements.DropUniqueConstraint(uc));
        if (tableDiff.PrimaryKeyToDrop is not null)
            sb.Append(Statements.DropPrimaryKey());

        AppendColumnsAlters(sb, tableDiff);

        if (tableDiff.PrimaryKeyToCreate is not null)
            sb.Append(Statements.AddPrimaryKey(tableDiff.PrimaryKeyToCreate));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
            sb.Append(Statements.AddUniqueConstraint(uc));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
            sb.Append(Statements.AddCheckConstraint(ck));
        foreach (ForeignKey fk in tableDiff.ForeignKeysToCreate)
            sb.Append(Statements.AddForeignKey(fk));

        if (sb.Length > 0)
            sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }

    private void AppendColumnsAlters(StringBuilder sb, TableDiff tableDiff)
    {
        foreach (Column column in tableDiff.ColumnsToDrop)
            sb.Append(Statements.DropColumn(column));

        foreach (ColumnDiff columnDiff in tableDiff.ColumnsToAlter)
        {
            MySQLColumnDiff cDiff = (MySQLColumnDiff)columnDiff;
            if (cDiff.DefinitionToSet is not null)
                sb.Append(Statements.AlterColumnDefinition(cDiff.DefinitionToSet));
            else if (cDiff.DefaultToSet is not null)
                sb.Append(Statements.SetColumnDefault(cDiff.NewColumnName, cDiff.DefaultToSet));
            else if (cDiff.DefaultToDrop is not null)
                sb.Append(Statements.DropColumnDefault(cDiff.NewColumnName));
        }

        foreach (Column column in tableDiff.ColumnsToAdd)
            sb.Append(Statements.AddColumn(column));
    }

    private static class Statements
    {
        public static string DefColumn(Column c) =>
$@"`{c.Name}` {c.DataType.Name}{Identity(c.Identity)} {Nullability(c.NotNull)}{Default(c.Default)}"
            ;
        public static string DefPrimaryKey(PrimaryKey pk) =>
$@"CONSTRAINT `{pk.Name}` PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"`{x}`"))})"
            ;
        public static string DefUniqueConstraint(UniqueConstraint uc) =>
$@"CONSTRAINT `{uc.Name}` UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"`{x}`"))})"
            ;
        public static string DefCheckConstraint(CheckConstraint ck) =>
$@"CONSTRAINT `{ck.Name}` CHECK ({ck.GetExpression()})"
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

        public static string AddColumn(Column c) =>
$@"
    ADD COLUMN {DefColumn(c)},"
            ;
        public static string DropColumn(Column c) =>
$@"
    DROP COLUMN `{c.Name}`,"
            ;
        public static string AlterColumnDefinition(Column c) =>
$@"
    MODIFY COLUMN {DefColumn(c)},"
            ;

        public static string SetColumnDefault(string cName, CodePiece dValue) =>
$@"
    ALTER COLUMN `{cName}` SET DEFAULT {dValue.Code},"
            ;
        public static string DropColumnDefault(string cName) =>
$@"
    ALTER COLUMN `{cName}` DROP DEFAULT,"
            ;

        public static string AddPrimaryKey(PrimaryKey pk) =>
$@"
    ADD PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"`{x}`"))}),"
            ;
        public static string DropPrimaryKey() =>
$@"
    DROP PRIMARY KEY,"
;

        public static string AddUniqueConstraint(UniqueConstraint uc) =>
$@"
    ADD {DefUniqueConstraint(uc)},"
            ;
        public static string DropUniqueConstraint(UniqueConstraint uc) =>
$@"
    DROP CONSTRAINT `{uc.Name}`,"
            ;

        public static string AddCheckConstraint(CheckConstraint ck) =>
$@"
    ADD {DefCheckConstraint(ck)},"
            ;
        public static string DropCheckConstraint(CheckConstraint ck) =>
$@"
    DROP CONSTRAINT `{ck.Name}`,"
            ;

        public static string AddForeignKey(ForeignKey fk) =>
$@"
    ADD {DefForeignKey(fk)},"
            ;
        public static string DropForeignKey(ForeignKey fk) =>
$@"
    DROP CONSTRAINT `{fk.Name}`,"
            ;

        private static string Identity(bool identity) =>
identity ? " AUTO_INCREMENT" : ""
            ;
        private static string Nullability(bool notNull) =>
notNull ? "NOT NULL" : "NULL"
            ;
        private static string Default(CodePiece dValue) =>
dValue is not null ? $@" DEFAULT {dValue.Code}" : ""
            ;
    }
}
