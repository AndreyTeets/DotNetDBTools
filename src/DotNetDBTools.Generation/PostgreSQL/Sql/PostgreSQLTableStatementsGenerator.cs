using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Generation.PostgreSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLTableStatementsGenerator : TableStatementsGenerator<PostgreSQLTable, PostgreSQLTableDiff>
{
    protected override string GetCreateSqlImpl(PostgreSQLTable table)
    {
        string res =
$@"{GetIdDeclarationText(table, 0)}CREATE TABLE ""{table.Name}""
(
{GetTableDefinitionsText(table)}
);";

        return res;
    }

    protected override string GetDropSqlImpl(PostgreSQLTable table)
    {
        return $@"DROP TABLE ""{table.Name}"";";
    }

    protected override string GetAlterSqlImpl(PostgreSQLTableDiff tableDiff)
    {
        StringBuilder sb = new();

        if (tableDiff.NewTable.Name != tableDiff.OldTable.Name)
            sb.AppendLine(Statements.RenameTable(tableDiff.OldTable.Name, tableDiff.NewTable.Name));

        foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns.Where(x => x.NewColumn.Name != x.OldColumn.Name))
            sb.AppendLine(Statements.RenameColumn(tableDiff.NewTable.Name, columnDiff.OldColumn.Name, columnDiff.NewColumn.Name));

        string tableAlters = GetTableAltersText(tableDiff);
        if (!string.IsNullOrEmpty(tableAlters))
            sb.AppendLine($@"ALTER TABLE ""{tableDiff.NewTable.Name}""{tableAlters};");

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
            sb.Append(Statements.DropPrimaryKey(tableDiff.PrimaryKeyToDrop));

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
        foreach (Column column in tableDiff.RemovedColumns)
            sb.Append(Statements.DropColumn(column));

        foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
        {
            if (columnDiff.DataTypeChanged)
                sb.Append(Statements.AlterColumnType(columnDiff.NewColumn));

            if (columnDiff.NewColumn.NotNull && !columnDiff.OldColumn.NotNull)
                sb.Append(Statements.SetColumnNotNull(columnDiff.NewColumn));
            else if (!columnDiff.NewColumn.NotNull && columnDiff.OldColumn.NotNull)
                sb.Append(Statements.DropColumnNotNull(columnDiff.NewColumn));

            bool defaultChagned = columnDiff.NewColumn.Default.Code != columnDiff.OldColumn.Default.Code;
            if (columnDiff.NewColumn.Default.Code is not null && defaultChagned)
                sb.Append(Statements.AddDefaultConstraint(columnDiff.NewColumn));
            else if (columnDiff.NewColumn.Default.Code is null && defaultChagned)
                sb.Append(Statements.DropDefaultConstraint(columnDiff.NewColumn));
        }

        foreach (Column column in tableDiff.AddedColumns)
            sb.Append(Statements.AddColumn(column));
    }

    private static class Statements
    {
        public static string DefColumn(Column c) =>
$@"""{c.Name}"" {c.DataType.GetQuotedName()}{Identity(c)} {Nullability(c)}{Default(c)}"
            ;
        public static string DefPrimaryKey(PrimaryKey pk) =>
$@"CONSTRAINT ""{pk.Name}"" PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"""{x}"""))})"
            ;
        public static string DefUniqueConstraint(UniqueConstraint uc) =>
$@"CONSTRAINT ""{uc.Name}"" UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"""{x}"""))})"
            ;
        public static string DefCheckConstraint(CheckConstraint ck) =>
$@"CONSTRAINT ""{ck.Name}"" {ck.GetCode()}"
            ;
        public static string DefForeignKey(ForeignKey fk) =>
$@"CONSTRAINT ""{fk.Name}"" FOREIGN KEY ({string.Join(", ", fk.ThisColumnNames.Select(x => $@"""{x}"""))})
        REFERENCES ""{fk.ReferencedTableName}"" ({string.Join(", ", fk.ReferencedTableColumnNames.Select(x => $@"""{x}"""))})
        ON UPDATE {fk.OnUpdate} ON DELETE {fk.OnDelete}"
            ;

        public static string RenameTable(string oldTableName, string newTableName) =>
$@"ALTER TABLE ""{oldTableName}"" RENAME TO ""{newTableName}"";"
            ;
        public static string RenameColumn(string tableName, string oldColumnName, string newColumnName) =>
$@"
ALTER TABLE ""{tableName}"" RENAME COLUMN ""{oldColumnName}"" TO ""{newColumnName}"";"
            ;

        public static string AddColumn(Column c) =>
$@"
    ADD COLUMN {DefColumn(c)},"
            ;
        public static string DropColumn(Column c) =>
$@"
    DROP COLUMN ""{c.Name}"","
            ;
        public static string AlterColumnType(Column c) =>
$@"
    ALTER COLUMN ""{c.Name}"" SET DATA TYPE {c.DataType.GetQuotedName()}
        USING (""{c.Name}""::text::{c.DataType.GetQuotedName()}),"
            ; // TODO Add TypeChangeConversions<srcTypeName,destTypeName,usingCode> in deploy/generation options?
        public static string SetColumnNotNull(Column c) =>
$@"
    ALTER COLUMN ""{c.Name}"" SET NOT NULL,"
            ;
        public static string DropColumnNotNull(Column c) =>
$@"
    ALTER COLUMN ""{c.Name}"" DROP NOT NULL,"
            ;
        public static string AddDefaultConstraint(Column c) =>
$@"
    ALTER COLUMN ""{c.Name}"" SET DEFAULT {c.GetCode()},"
            ;
        public static string DropDefaultConstraint(Column c) =>
$@"
    ALTER COLUMN ""{c.Name}"" DROP DEFAULT,"
            ;

        public static string AddPrimaryKey(PrimaryKey pk) =>
$@"
    ADD {DefPrimaryKey(pk)},"
            ;
        public static string DropPrimaryKey(PrimaryKey pk) =>
$@"
    DROP CONSTRAINT ""{pk.Name}"","
            ;

        public static string AddUniqueConstraint(UniqueConstraint uc) =>
$@"
    ADD {DefUniqueConstraint(uc)},"
            ;
        public static string DropUniqueConstraint(UniqueConstraint uc) =>
$@"
    DROP CONSTRAINT ""{uc.Name}"","
            ;

        public static string AddCheckConstraint(CheckConstraint ck) =>
$@"
    ADD {DefCheckConstraint(ck)},"
            ;
        public static string DropCheckConstraint(CheckConstraint ck) =>
$@"
    DROP CONSTRAINT ""{ck.Name}"","
            ;

        public static string AddForeignKey(ForeignKey fk) =>
$@"
    ADD {DefForeignKey(fk)},"
            ;
        public static string DropForeignKey(ForeignKey fk) =>
$@"
    DROP CONSTRAINT ""{fk.Name}"","
            ;

        private static string Identity(Column c) =>
c.Identity ? " GENERATED ALWAYS AS IDENTITY" : ""
            ;
        private static string Nullability(Column c) =>
c.NotNull ? "NOT NULL" : "NULL"
            ;
        private static string Default(Column c) =>
c.GetCode() is not null ? $@" DEFAULT {c.GetCode()}" : ""
            ;
    }
}
