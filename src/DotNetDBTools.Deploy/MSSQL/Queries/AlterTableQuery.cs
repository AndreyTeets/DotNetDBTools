using System.Collections.Generic;
using System.Text;
using DotNetDBTools.Deploy.Shared;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class AlterTableQuery : IQuery
    {
        private const string NewTableMetadataParameterName = "@NewTableMetadata";
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public AlterTableQuery(MSSQLTableDiff tableDiff, string newTableMetadataParameterValue)
        {
            _sql = GetSql(tableDiff);
            _parameters = new List<QueryParameter>
            {
                new QueryParameter(NewTableMetadataParameterName, newTableMetadataParameterValue),
            };
        }

        private static string GetSql(MSSQLTableDiff tableDiff)
        {
            StringBuilder sb = new();
            sb.Append(
$@"EXEC sp_rename '{tableDiff.OldTable.Name}', '{tableDiff.NewTable.Name}';

");

            foreach (MSSQLColumnInfo column in tableDiff.RemovedColumns)
            {
                sb.Append(
$@"ALTER TABLE {tableDiff.NewTable.Name} DROP COLUMN {column.Name};

");
            }

            foreach (MSSQLColumnDiff columnDiff in tableDiff.ChangedColumns)
            {
                sb.Append(
$@"EXEC sp_rename '{tableDiff.NewTable.Name}.{columnDiff.OldColumn.Name}', '{columnDiff.NewColumn.Name}', 'COLUMN';
ALTER TABLE {tableDiff.NewTable.Name} ALTER COLUMN {columnDiff.NewColumn.Name} {columnDiff.NewColumn.DataType};

");
            }

            foreach (MSSQLColumnInfo column in tableDiff.AddedColumns)
            {
                sb.Append(
$@"ALTER TABLE {tableDiff.NewTable.Name} ADD {column.Name} {column.DataType} UNIQUE;

");
            }

            sb.Append(
$@"UPDATE {DNDBTSysTables.DNDBTDbObjects} SET
    {DNDBTSysTables.DNDBTDbObjects.Name} = '{tableDiff.NewTable.Name}',
    {DNDBTSysTables.DNDBTDbObjects.Metadata} = {NewTableMetadataParameterName}
WHERE {DNDBTSysTables.DNDBTDbObjects.ID} = '{tableDiff.NewTable.ID}';");

            return sb.ToString();
        }
    }
}
