using System.Linq;
using System.Text;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.PostgreSQL.PostgreSQLQueriesHelper;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLAlterTableQuery : AlterTableQuery
{
    public PostgreSQLAlterTableQuery(TableDiff tableDiff)
        : base(tableDiff) { }

    protected override string GetSql(TableDiff tableDiff)
    {
        StringBuilder sb = new();

        if (tableDiff.NewTable.Name != tableDiff.OldTable.Name)
            sb.AppendLine(Queries.RenameTable(tableDiff.OldTable.Name, tableDiff.NewTable.Name));

        foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns.Where(x => x.NewColumn.Name != x.OldColumn.Name))
            sb.AppendLine(Queries.RenameColumn(tableDiff.NewTable.Name, columnDiff.OldColumn.Name, columnDiff.NewColumn.Name));

        string tableAlters = GetTableAlter(tableDiff);
        if (!string.IsNullOrEmpty(tableAlters))
            sb.AppendLine($@"ALTER TABLE ""{tableDiff.NewTable.Name}""{tableAlters};");

        return sb.ToString();
    }

    private static string GetTableAlter(TableDiff tableDiff)
    {
        StringBuilder sb = new();

        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToDrop)
            sb.Append(Queries.DropCheckConstraint(ck.Name));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
            sb.Append(Queries.DropUniqueConstraint(uc.Name));
        if (tableDiff.PrimaryKeyToDrop is not null)
            sb.Append(Queries.DropPrimaryKey(tableDiff.PrimaryKeyToDrop.Name));

        AppendColumnsAlters(sb, tableDiff);

        if (tableDiff.PrimaryKeyToCreate is not null)
            sb.Append(Queries.AddPrimaryKey(tableDiff.PrimaryKeyToCreate));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
            sb.Append(Queries.AddUniqueConstraint(uc));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
            sb.Append(Queries.AddCheckConstraint(ck));

        if (sb.Length > 0)
            sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }

    private static void AppendColumnsAlters(StringBuilder sb, TableDiff tableDiff)
    {
        foreach (Column column in tableDiff.RemovedColumns)
            sb.Append(Queries.DropColumn(column.Name));

        foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
        {
            // TODO if (columnDiff.NewColumn.DataType.Name != columnDiff.OldColumn.DataType.Name)
            // Need to track if custom data type was changed (and so being recreated).
            sb.Append(Queries.AlterColumnType(columnDiff.NewColumn));

            if (columnDiff.NewColumn.NotNull && !columnDiff.OldColumn.NotNull)
                sb.Append(Queries.SetColumnNotNull(columnDiff.NewColumn));
            else if (!columnDiff.NewColumn.NotNull && columnDiff.OldColumn.NotNull)
                sb.Append(Queries.DropColumnNotNull(columnDiff.NewColumn.Name));

            if (columnDiff.NewColumn.Default.Code != columnDiff.OldColumn.Default.Code)
                sb.Append(Queries.AddDefaultConstraint(columnDiff.NewColumn));
        }

        foreach (Column column in tableDiff.AddedColumns)
            sb.Append(Queries.AddColumn(column));
    }

    private static class Queries
    {
        public static string RenameTable(string oldTableName, string newTableName) =>
$@"ALTER TABLE ""{oldTableName}"" RENAME TO ""{newTableName}"";"
            ;
        public static string RenameColumn(string tableName, string oldColumnName, string newColumnName) =>
$@"
ALTER TABLE ""{tableName}"" RENAME COLUMN ""{oldColumnName}"" TO ""{newColumnName}"";"
            ;

        public static string AddColumn(Column c) =>
$@"
    ADD COLUMN ""{c.Name}"" {c.DataType.GetQuotedName()}{GetIdentityStatement(c)} {GetNullabilityStatement(c)}{GetDefaultValStatement(c)},"
            ;
        public static string DropColumn(string columnName) =>
$@"
    DROP COLUMN ""{columnName}"","
            ;
        public static string AlterColumnType(Column c) =>
$@"
    ALTER COLUMN ""{c.Name}"" SET DATA TYPE {c.DataType.GetQuotedName()}
        USING (""{c.Name}""::text::{c.DataType.GetQuotedName()}),"
            ; // TODO Add TypeChangeConversionsList<DataType(srcType),DataType(destType),string(usingCode)> in definition for columns?
        public static string SetColumnNotNull(Column c) =>
$@"
    ALTER COLUMN ""{c.Name}"" SET NOT NULL,"
;
        public static string DropColumnNotNull(string columnName) =>
$@"
    ALTER COLUMN ""{columnName}"" DROP NOT NULL,"
;

        public static string AddPrimaryKey(PrimaryKey pk) =>
$@"
    ADD CONSTRAINT ""{pk.Name}"" PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"""{x}"""))}),"
            ;
        public static string DropPrimaryKey(string pkName) =>
$@"
    DROP CONSTRAINT ""{pkName}"","
            ;

        public static string AddUniqueConstraint(UniqueConstraint uc) =>
$@"
    ADD CONSTRAINT ""{uc.Name}"" UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"""{x}"""))}),"
            ;
        public static string DropUniqueConstraint(string ucName) =>
$@"
    DROP CONSTRAINT ""{ucName}"","
            ;

        public static string AddCheckConstraint(CheckConstraint ck) =>
$@"
    ADD CONSTRAINT ""{ck.Name}"" {ck.GetCode()},"
            ;
        public static string DropCheckConstraint(string ckName) =>
$@"
    DROP CONSTRAINT ""{ckName}"","
            ;

        public static string AddDefaultConstraint(Column c) =>
$@"
    ALTER COLUMN ""{c.Name}"" SET DEFAULT {c.Default.Code},"
            ;
        public static string DropDefaultConstraint(string columnName) =>
$@"
    ALTER COLUMN ""{columnName}"" DROP DEFAULT,"
            ;
    }
}
