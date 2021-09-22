using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

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
            sb.Append(
$@"EXEC sp_rename '{tableDiff.OldTable.Name}', '{tableDiff.NewTable.Name}';");
            sb.AppendLine();

            AppendColumnsAlters(sb, tableDiff);
            sb.AppendLine();

            AppendForeignKeysAlters(sb, tableDiff);
            sb.AppendLine();

            return sb.ToString();
        }

        private static void AppendColumnsAlters(StringBuilder sb, MSSQLTableDiff tableDiff)
        {
            foreach (ColumnInfo column in tableDiff.RemovedColumns)
            {
                sb.Append(
$@"
ALTER TABLE {tableDiff.NewTable.Name} DROP COLUMN {column.Name};");
            }

            foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
            {
                sb.Append(
$@"
EXEC sp_rename '{tableDiff.NewTable.Name}.{columnDiff.OldColumn.Name}', '{columnDiff.NewColumn.Name}', 'COLUMN';
ALTER TABLE {tableDiff.NewTable.Name} ALTER COLUMN {columnDiff.NewColumn.Name} {MSSQLSqlTypeMapper.GetSqlType(columnDiff.NewColumn.DataType)} {GetNullability(columnDiff.NewColumn)};");

                if (columnDiff.NewColumn.Unique && !columnDiff.OldColumn.Unique)
                {
                    sb.Append(
$@"
ALTER TABLE {tableDiff.NewTable.Name} ADD CONSTRAINT UQ_{tableDiff.NewTable.Name}_{columnDiff.NewColumn.Name} UNIQUE ({columnDiff.NewColumn.Name});");
                }
            }

            foreach (ColumnInfo column in tableDiff.AddedColumns)
            {
                sb.Append(
$@"
ALTER TABLE {tableDiff.NewTable.Name} ADD {column.Name} {MSSQLSqlTypeMapper.GetSqlType(column.DataType)} {GetNullability(column)};");

                if (column.Unique)
                {
                    sb.Append(
$@"
ALTER TABLE {tableDiff.NewTable.Name} ADD CONSTRAINT UQ_{tableDiff.NewTable.Name}_{column.Name} UNIQUE ({column.Name});");
                }
            }
        }

        private static void AppendForeignKeysAlters(StringBuilder sb, MSSQLTableDiff tableDiff)
        {
            foreach (MSSQLForeignKeyInfo fk in tableDiff.RemovedForeignKeys.Concat(tableDiff.ChangedForeignKeys.Select(x => x.OldForeignKey)))
            {
                sb.Append(
$@"
ALTER TABLE {tableDiff.NewTable.Name} DROP CONSTRAINT {fk.Name};");
            }

            foreach (MSSQLForeignKeyInfo fk in tableDiff.AddedForeignKeys.Concat(tableDiff.ChangedForeignKeys.Select(x => x.NewForeignKey)))
            {
                sb.Append(
$@"
ALTER TABLE {tableDiff.NewTable.Name} ADD CONSTRAINT {fk.Name} FOREIGN KEY ({string.Join(",", fk.ThisColumnNames)}
    REFERENCES {fk.ForeignTableName}({string.Join(",", fk.ForeignColumnNames)})
    ON UPDATE {MapActionName(fk.OnUpdate)} ON DELETE {MapActionName(fk.OnDelete)};");
            }
        }

        private static string GetNullability(ColumnInfo column) =>
            column.Nullable switch
            {
                true => "NULL",
                false => "NOT NULL",
            };

        private static string MapActionName(string modelActionName) =>
            modelActionName switch
            {
                "NoAction" => "NO ACTION",
                "Cascade" => "CASCADE",
                _ => throw new InvalidOperationException($"Invalid modelActionName: '{modelActionName}'")
            };
    }
}
