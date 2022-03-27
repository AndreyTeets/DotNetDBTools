using System.Text;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using static DotNetDBTools.Deploy.MSSQL.MSSQLQueriesHelper;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL;

internal class MSSQLAlterTableQuery : AlterTableQuery
{
    public MSSQLAlterTableQuery(TableDiff tableDiff)
        : base(tableDiff) { }

    protected override string GetSql(TableDiff tableDiff)
    {
        StringBuilder sb = new();

        if (tableDiff.OldTable.Name != tableDiff.NewTable.Name)
            sb.Append(Queries.RenameTable(tableDiff.OldTable.Name, tableDiff.NewTable.Name));

        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToDrop)
            sb.Append(Queries.DropCheckConstraint(tableDiff.NewTable.Name, ck.Name));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
            sb.Append(Queries.DropUniqueConstraint(tableDiff.NewTable.Name, uc.Name));
        if (tableDiff.PrimaryKeyToDrop is not null)
            sb.Append(Queries.DropPrimaryKey(tableDiff.NewTable.Name, tableDiff.PrimaryKeyToDrop.Name));

        AppendColumnsAlters(sb, tableDiff);

        if (tableDiff.PrimaryKeyToCreate is not null)
            sb.Append(Queries.AddPrimaryKey(tableDiff.NewTable.Name, tableDiff.PrimaryKeyToCreate));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
            sb.Append(Queries.AddUniqueConstraint(tableDiff.NewTable.Name, uc));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
            sb.Append(Queries.AddCheckConstraint(tableDiff.NewTable.Name, ck));

        return sb.ToString();
    }

    private static void AppendColumnsAlters(StringBuilder sb, TableDiff tableDiff)
    {
        foreach (Column column in tableDiff.RemovedColumns)
        {
            if (column.Default.Code is not null)
                sb.Append(Queries.DropDefaultConstraint(tableDiff.NewTable.Name, column));
            sb.Append(Queries.DropColumn(tableDiff.NewTable.Name, column.Name));
        }

        foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
        {
            if (columnDiff.OldColumn.Default.Code is not null)
                sb.Append(Queries.DropDefaultConstraint(tableDiff.NewTable.Name, columnDiff.OldColumn));

            if (columnDiff.OldColumn.Name != columnDiff.NewColumn.Name)
                sb.Append(Queries.RenameColumn(tableDiff.NewTable.Name, columnDiff.OldColumn.Name, columnDiff.NewColumn.Name));
            sb.Append(Queries.AlterColumnTypeAndNullability(tableDiff.NewTable.Name, columnDiff.NewColumn));

            if (columnDiff.NewColumn.Default.Code is not null)
                sb.Append(Queries.AddDefaultConstraint(tableDiff.NewTable.Name, columnDiff.NewColumn));
        }

        foreach (Column column in tableDiff.AddedColumns)
        {
            sb.Append(Queries.AddColumn(tableDiff.NewTable.Name, column));
            if (column.Default.Code is not null)
                sb.Append(Queries.AddDefaultConstraint(tableDiff.NewTable.Name, column));
        }
    }

    private static class Queries
    {
        public static string RenameTable(string oldTableName, string newTableName) =>
$@"EXEC sp_rename '{oldTableName}', '{newTableName}';"
            ;
        public static string RenameColumn(string tableName, string oldColumnName, string newColumnName) =>
$@"
EXEC sp_rename '{tableName}.{oldColumnName}', '{newColumnName}', 'COLUMN';"
            ;

        public static string AddColumn(string tableName, Column c) =>
$@"
ALTER TABLE {tableName} ADD {c.Name} {c.DataType.Name}{GetIdentityStatement(c)} {GetNullabilityStatement(c)};"
            ;
        public static string DropColumn(string tableName, string columnName) =>
$@"
ALTER TABLE {tableName} DROP COLUMN {columnName};"
            ;
        public static string AlterColumnTypeAndNullability(string tableName, Column c) =>
$@"
ALTER TABLE {tableName} ALTER COLUMN {c.Name} {c.DataType.Name} {GetNullabilityStatement(c)};"
            ;

        public static string AddPrimaryKey(string tableName, PrimaryKey pk) =>
$@"
ALTER TABLE {tableName} ADD CONSTRAINT {pk.Name} PRIMARY KEY ({string.Join(", ", pk.Columns)});"
            ;
        public static string DropPrimaryKey(string tableName, string pkName) =>
$@"
ALTER TABLE {tableName} DROP CONSTRAINT {pkName};"
            ;

        public static string AddUniqueConstraint(string tableName, UniqueConstraint uc) =>
$@"
ALTER TABLE {tableName} ADD CONSTRAINT {uc.Name} UNIQUE ({string.Join(", ", uc.Columns)});"
            ;
        public static string DropUniqueConstraint(string tableName, string ucName) =>
$@"
ALTER TABLE {tableName} DROP CONSTRAINT {ucName};"
            ;

        public static string AddCheckConstraint(string tableName, CheckConstraint ck) =>
$@"
ALTER TABLE {tableName} ADD CONSTRAINT {ck.Name} {ck.GetCode()};"
            ;
        public static string DropCheckConstraint(string tableName, string ckName) =>
$@"
ALTER TABLE {tableName} DROP CONSTRAINT {ckName};"
            ;

        public static string AddDefaultConstraint(string tableName, Column c) =>
$@"
ALTER TABLE {tableName} ADD CONSTRAINT {((MSSQLColumn)c).DefaultConstraintName} DEFAULT {c.Default.Code} FOR {c.Name};"
            ;
        public static string DropDefaultConstraint(string tableName, Column c) =>
$@"
ALTER TABLE [{tableName}] DROP CONSTRAINT {((MSSQLColumn)c).DefaultConstraintName};"
            ;
    }
}
