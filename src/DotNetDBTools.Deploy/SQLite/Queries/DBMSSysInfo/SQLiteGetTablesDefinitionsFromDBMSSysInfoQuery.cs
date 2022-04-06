using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;

internal class SQLiteGetTablesDefinitionsFromDBMSSysInfoQuery : IQuery
{
    public string Sql =>
$@"SELECT
    sm.name AS [{nameof(TableRecord.TableName)}],
    sm.sql AS [{nameof(TableRecord.TableDefinition)}]
FROM sqlite_master sm
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

    public class TableRecord
    {
        public string TableName { get; set; }
        public string TableDefinition { get; set; }
    }
}
