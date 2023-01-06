using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;

internal class SQLiteGetTablesDefinitionsFromDBMSSysInfoQuery : NoParametersQuery
{
    public override string Sql =>
$@"SELECT
    sm.name AS [{nameof(TableRecord.TableName)}],
    sm.sql AS [{nameof(TableRecord.TableDefinition)}]
FROM sqlite_master sm
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public class TableRecord
    {
        public string TableName { get; set; }
        public string TableDefinition { get; set; }
    }
}
