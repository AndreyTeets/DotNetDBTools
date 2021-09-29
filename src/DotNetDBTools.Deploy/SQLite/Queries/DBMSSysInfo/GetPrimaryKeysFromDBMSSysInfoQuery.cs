using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo
{
    internal class GetPrimaryKeysFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    sm.name AS {nameof(PrimaryKeyRecord.TableName)},
    'PK_' || sm.name AS {nameof(PrimaryKeyRecord.ConstraintName)},
    ii.name AS {nameof(PrimaryKeyRecord.ColumnName)},
    ii.seqno AS {nameof(PrimaryKeyRecord.ColumnPosition)}
FROM sqlite_master sm
INNER JOIN pragma_index_list(sm.name) il
INNER JOIN pragma_index_info(il.name) ii
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name != '{DNDBTSysTables.DNDBTDbObjects}'
    AND il.origin = 'pk'
UNION
SELECT
    sm.name AS {nameof(PrimaryKeyRecord.TableName)},
    'PK_' || sm.name AS {nameof(PrimaryKeyRecord.ConstraintName)},
    ti.name AS {nameof(PrimaryKeyRecord.ColumnName)},
    0 AS {nameof(PrimaryKeyRecord.ColumnPosition)}
FROM sqlite_master sm
INNER JOIN pragma_table_info(sm.name) ti
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name != '{DNDBTSysTables.DNDBTDbObjects}'
    AND ti.pk = 1
    AND lower(ti.type) = 'integer';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class PrimaryKeyRecord : PrimaryKeysBuilder.PrimaryKeyRecord { }
        internal static class ResultsInterpreter
        {
            public static void BuildTablesPrimaryKeys(
                Dictionary<string, TableInfo> tables,
                IEnumerable<PrimaryKeyRecord> primaryKeyRecords)
            {
                PrimaryKeysBuilder.BuildTablesPrimaryKeys(tables, primaryKeyRecords);
            }
        }
    }
}
