using System.Collections.Generic;
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
$@"EXEC sp_rename '{tableDiff.OldTable.Name}', '{tableDiff.NewTable.Name}';

");

            foreach (ColumnInfo column in tableDiff.RemovedColumns)
            {
                sb.Append(
$@"ALTER TABLE {tableDiff.NewTable.Name} DROP COLUMN {column.Name};

");
            }

            foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
            {
                sb.Append(
$@"EXEC sp_rename '{tableDiff.NewTable.Name}.{columnDiff.OldColumn.Name}', '{columnDiff.NewColumn.Name}', 'COLUMN';
ALTER TABLE {tableDiff.NewTable.Name} ALTER COLUMN {columnDiff.NewColumn.Name} {MSSQLSqlTypeMapper.GetSqlType(columnDiff.NewColumn)};

");
            }

            foreach (ColumnInfo column in tableDiff.AddedColumns)
            {
                sb.Append(
$@"ALTER TABLE {tableDiff.NewTable.Name} ADD {column.Name} {MSSQLSqlTypeMapper.GetSqlType(column)} UNIQUE;

");
            }

            return sb.ToString();
        }
    }
}
