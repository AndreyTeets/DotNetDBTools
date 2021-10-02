using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo
{
    internal class GetPrimaryKeysFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.name AS {nameof(PrimaryKeyRecord.TableName)},
    i.name AS {nameof(PrimaryKeyRecord.ConstraintName)},
    c.name AS {nameof(PrimaryKeyRecord.ColumnName)},
    ic.key_ordinal AS {nameof(PrimaryKeyRecord.ColumnPosition)}
FROM sys.tables t
INNER JOIN sys.columns c
      ON c.object_id = t.object_id
INNER JOIN sys.indexes i
    ON i.object_id = t.object_id
INNER JOIN sys.index_columns ic
    ON ic.object_id = t.object_id
        AND ic.index_id = i.index_id
        AND ic.column_id = c.column_id
WHERE i.is_primary_key = 1
    AND t.name != '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class PrimaryKeyRecord : PrimaryKeysBuilder.PrimaryKeyRecord { }
        internal static class ResultsInterpreter
        {
            public static void BuildTablesPrimaryKeys(
                Dictionary<string, Table> tables,
                IEnumerable<PrimaryKeyRecord> primaryKeyRecords)
            {
                PrimaryKeysBuilder.BuildTablesPrimaryKeys(tables, primaryKeyRecords);
            }
        }
    }
}
