using System.Linq;
using System.Text;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.MySQL.MySQLQueriesHelper;

namespace DotNetDBTools.Deploy.MySQL.Queries.DDL;

internal class MySQLAlterTableQuery : AlterTableQuery
{
    public MySQLAlterTableQuery(TableDiff tableDiff)
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
            sb.AppendLine(tableAlters);

        return sb.ToString();
    }

    private static string GetTableAlter(TableDiff tableDiff)
    {
        StringBuilder sb = new();

        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToDrop)
            sb.Append(Queries.DropCheckConstraint(tableDiff.NewTable.Name, ck.Name));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
            sb.Append(Queries.DropUniqueConstraint(tableDiff.NewTable.Name, uc.Name));

        if (tableDiff.PrimaryKeyToDrop is not null)
        {
            Column identityColumn = tableDiff.OldTable.Columns.SingleOrDefault(c => c.Identity);
            if (tableDiff.PrimaryKeyToDrop.Columns.Any(c => c == identityColumn?.Name))
                sb.Append(Queries.DropPrimaryKeyAndColumnIdentityAttribute(tableDiff.NewTable.Name, identityColumn));
            else
                sb.Append(Queries.DropPrimaryKey(tableDiff.NewTable.Name));
        }
        bool addedPk = AppendColumnsAlters(sb, tableDiff);
        if (tableDiff.PrimaryKeyToCreate is not null && !addedPk)
            sb.Append(Queries.AddPrimaryKey(tableDiff.NewTable.Name, tableDiff.PrimaryKeyToCreate));

        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
            sb.Append(Queries.AddUniqueConstraint(tableDiff.NewTable.Name, uc));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
            sb.Append(Queries.AddCheckConstraint(tableDiff.NewTable.Name, ck));

        return sb.ToString();
    }

    private static bool AppendColumnsAlters(StringBuilder sb, TableDiff tableDiff)
    {
        foreach (Column column in tableDiff.RemovedColumns)
            sb.Append(Queries.DropColumn(tableDiff.NewTable.Name, column.Name));

        foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
        {
            bool defaultChagned = columnDiff.NewColumn.Default.Code != columnDiff.OldColumn.Default.Code;
            if (columnDiff.DataTypeChanged || columnDiff.NewColumn.NotNull != columnDiff.OldColumn.NotNull)
                sb.Append(Queries.AlterColumnDefinition(tableDiff.NewTable.Name, columnDiff.NewColumn));
            else if (columnDiff.NewColumn.Default.Code is not null && defaultChagned)
                sb.Append(Queries.AddDefaultConstraint(tableDiff.NewTable.Name, columnDiff.NewColumn));
            else if (columnDiff.NewColumn.Default.Code is null && defaultChagned)
                sb.Append(Queries.DropDefaultConstraint(tableDiff.NewTable.Name, columnDiff.NewColumn.Name));
        }

        bool addedPk = false;
        foreach (Column column in tableDiff.AddedColumns)
        {
            if (tableDiff.PrimaryKeyToCreate is not null && tableDiff.PrimaryKeyToCreate.Columns.Any(c => c == column.Name))
            {
                sb.Append(Queries.AddColumnAsPrimaryKey(tableDiff.NewTable.Name, column, tableDiff.PrimaryKeyToCreate));
                addedPk = true;
            }
            else
            {
                sb.Append(Queries.AddColumn(tableDiff.NewTable.Name, column));
            }
        }
        return addedPk;
    }

    private static class Queries
    {
        public static string RenameTable(string oldTableName, string newTableName) =>
$@"RENAME TABLE `{oldTableName}` TO `{newTableName}`;"
            ;
        public static string RenameColumn(string tableName, string oldColumnName, string newColumnName) =>
$@"
ALTER TABLE `{tableName}` RENAME COLUMN `{oldColumnName}` TO `{newColumnName}`;"
            ;

        public static string AddColumn(string tableName, Column c) =>
$@"
ALTER TABLE `{tableName}` ADD COLUMN `{c.Name}` {c.DataType.Name}{GetIdentityStatement(c)} {GetNullabilityStatement(c)}{GetDefaultValStatement(c)};"
            ;
        public static string AddColumnAsPrimaryKey(string tableName, Column c, PrimaryKey pk) =>
$@"
ALTER TABLE `{tableName}` ADD COLUMN `{c.Name}` {c.DataType.Name}{GetIdentityStatement(c)} {GetNullabilityStatement(c)},
     ADD PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"`{x}`"))});"
;
        public static string DropColumn(string tableName, string columnName) =>
$@"
ALTER TABLE `{tableName}` DROP COLUMN `{columnName}`;"
            ;
        public static string AlterColumnDefinition(string tableName, Column c) =>
$@"
ALTER TABLE `{tableName}` MODIFY COLUMN `{c.Name}` {c.DataType.Name} {GetNullabilityStatement(c)}{GetDefaultValStatement(c)};"
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
    MODIFY COLUMN `{identityColumn.Name}` {identityColumn.DataType.Name} {GetNullabilityStatement(identityColumn)};"
            ;

        public static string AddUniqueConstraint(string tableName, UniqueConstraint uc) =>
$@"
ALTER TABLE `{tableName}` ADD CONSTRAINT `{uc.Name}` UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"`{x}`"))});"
            ;
        public static string DropUniqueConstraint(string tableName, string ucName) =>
$@"
ALTER TABLE `{tableName}` DROP CONSTRAINT `{ucName}`;"
            ;

        public static string AddCheckConstraint(string tableName, CheckConstraint ck) =>
$@"
ALTER TABLE `{tableName}` ADD CONSTRAINT `{ck.Name}` {ck.GetCode()};"
            ;
        public static string DropCheckConstraint(string tableName, string ckName) =>
$@"
ALTER TABLE `{tableName}` DROP CONSTRAINT `{ckName}`;"
            ;

        public static string AddDefaultConstraint(string tableName, Column c) =>
$@"
ALTER TABLE `{tableName}` ALTER COLUMN `{c.Name}` SET DEFAULT {c.Default.Code};"
            ;
        public static string DropDefaultConstraint(string tableName, string cName) =>
$@"
ALTER TABLE `{tableName}` ALTER COLUMN `{cName}` DROP DEFAULT;"
            ;
    }
}
