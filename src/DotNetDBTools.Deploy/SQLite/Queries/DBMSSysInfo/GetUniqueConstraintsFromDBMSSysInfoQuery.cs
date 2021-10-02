using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo
{
    internal class GetUniqueConstraintsFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    sm.name AS {nameof(UniqueConstraintRecord.TableName)},
    'UQ_' || sm.name || '_' || il.seq AS {nameof(UniqueConstraintRecord.ConstraintName)},
    ii.name AS {nameof(UniqueConstraintRecord.ColumnName)},
    ii.seqno AS {nameof(UniqueConstraintRecord.ColumnPosition)}
FROM sqlite_master sm
INNER JOIN pragma_index_list(sm.name) il
INNER JOIN pragma_index_info(il.name) ii
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name != '{DNDBTSysTables.DNDBTDbObjects}'
    AND il.origin = 'u';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class UniqueConstraintRecord : UniqueConstraintsBuilder.UniqueConstraintRecord { }
        internal static class ResultsInterpreter
        {
            public static void BuildTablesUniqueConstraints(
                Dictionary<string, Table> tables,
                IEnumerable<UniqueConstraintRecord> uniqueConstraintRecords)
            {
                UniqueConstraintsBuilder.BuildTablesUniqueConstraints(tables, uniqueConstraintRecords);
            }
        }
    }
}
