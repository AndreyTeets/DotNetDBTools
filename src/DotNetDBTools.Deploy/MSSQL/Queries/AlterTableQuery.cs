using System.Collections.Generic;
using System.Text;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using static DotNetDBTools.Deploy.MSSQL.MSSQLSqlTypeMapper;
using static DotNetDBTools.Deploy.MSSQL.MSSQLQueriesHelper;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class AlterTableQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public AlterTableQuery(MSSQLTableDiff tableDiff)
        {
            _sql = GetSql(tableDiff);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(MSSQLTableDiff tableDiff)
        {
            StringBuilder sb = new();

            sb.Append(Queries.RenameTable(tableDiff.OldTable.Name, tableDiff.NewTable.Name));
            sb.AppendLine();

            foreach (UniqueConstraintInfo uc in tableDiff.RemovedUniqueConstraints)
                sb.Append(Queries.DropUniqueConstraint(tableDiff.NewTable.Name, uc.Name));
            if (tableDiff.RemovedPrimaryKey is not null)
                sb.Append(Queries.DropPrimaryKey(tableDiff.NewTable.Name, tableDiff.RemovedPrimaryKey.Name));
            sb.AppendLine();

            AppendColumnsAlters(sb, tableDiff);
            sb.AppendLine();

            if (tableDiff.AddedPrimaryKey is not null)
                sb.Append(Queries.AddPrimaryKey(tableDiff.NewTable.Name, tableDiff.AddedPrimaryKey));
            foreach (UniqueConstraintInfo uc in tableDiff.AddedUniqueConstraints)
                sb.Append(Queries.AddUniqueConstraint(tableDiff.NewTable.Name, uc));
            sb.AppendLine();

            return sb.ToString();
        }

        private static void AppendColumnsAlters(StringBuilder sb, MSSQLTableDiff tableDiff)
        {
            foreach (ColumnInfo column in tableDiff.RemovedColumns)
            {
                if (column.Default is not null)
                    sb.Append(Queries.DropDefaultConstraint(tableDiff.NewTable.Name, column.Name));
                sb.Append(Queries.DropColumn(tableDiff.NewTable.Name, column.Name));
            }

            foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
            {
                if (columnDiff.OldColumn.Default is not null)
                    sb.Append(Queries.DropDefaultConstraint(tableDiff.NewTable.Name, columnDiff.OldColumn.Name));

                sb.Append(Queries.RenameColumn(tableDiff.NewTable.Name, columnDiff.OldColumn.Name, columnDiff.NewColumn.Name));
                sb.Append(Queries.AlterColumnTypeAndNullability(tableDiff.NewTable.Name, columnDiff.NewColumn));

                if (columnDiff.NewColumn.Default is not null)
                    sb.Append(Queries.AddDefaultConstraint(tableDiff.NewTable.Name, columnDiff.NewColumn));
            }

            foreach (ColumnInfo column in tableDiff.AddedColumns)
            {
                sb.Append(Queries.AddColumn(tableDiff.NewTable.Name, column));
                if (column.Default is not null)
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

            public static string AddColumn(string tableName, ColumnInfo column) =>
$@"
ALTER TABLE {tableName} ADD {column.Name} {GetSqlType(column.DataType)} {GetNullabilityStatement(column)};"
                ;
            public static string DropColumn(string tableName, string columnName) =>
$@"
ALTER TABLE {tableName} DROP COLUMN {columnName};"
                ;
            public static string AlterColumnTypeAndNullability(string tableName, ColumnInfo column) =>
$@"
ALTER TABLE {tableName} ALTER COLUMN {column.Name} {GetSqlType(column.DataType)} {GetNullabilityStatement(column)};"
                ;

            public static string AddPrimaryKey(string tableName, PrimaryKeyInfo pk) =>
$@"
ALTER TABLE {tableName} ADD CONSTRAINT {pk.Name} PRIMARY KEY ({string.Join(", ", pk.Columns)});"
                ;
            public static string DropPrimaryKey(string tableName, string pkName) =>
$@"
ALTER TABLE {tableName} DROP CONSTRAINT {pkName};"
                ;

            public static string AddUniqueConstraint(string tableName, UniqueConstraintInfo uc) =>
$@"
ALTER TABLE {tableName} ADD CONSTRAINT {uc.Name} UNIQUE ({string.Join(", ", uc.Columns)});"
                ;
            public static string DropUniqueConstraint(string tableName, string ucName) =>
$@"
ALTER TABLE {tableName} DROP CONSTRAINT {ucName};"
                ;

            public static string AddDefaultConstraint(string tableName, ColumnInfo column) =>
$@"
ALTER TABLE {tableName} ADD CONSTRAINT DF_{tableName}_{column.Name} DEFAULT {QuoteDefaultValue(column.Default)} FOR {column.Name};"
                ;
            public static string DropDefaultConstraint(string tableName, string columnName) =>
$@"
DECLARE @DropDefaultConstraint_{tableName}_{columnName}_SqlText NVARCHAR(MAX) =
(
    SELECT
        'ALTER TABLE [{tableName}] DROP CONSTRAINT [' + dc.name + '];'
    FROM sys.tables t
    INNER JOIN sys.columns c
        ON c.object_id = t.object_id
    INNER JOIN sys.default_constraints dc
        ON dc.object_id = c.default_object_id
    WHERE t.name = '{tableName}'
        AND c.name = '{columnName}'
);
EXEC (@DropDefaultConstraint_{tableName}_{columnName}_SqlText);
--ALTER TABLE [{tableName}] DROP CONSTRAINT [DF_{tableName}_{columnName}];" // TODO DefaultConstraintName in columns
                ;
        }
    }
}
