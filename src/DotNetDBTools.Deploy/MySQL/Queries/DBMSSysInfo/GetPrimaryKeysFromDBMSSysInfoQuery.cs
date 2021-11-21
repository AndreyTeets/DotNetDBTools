using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo
{
    internal class GetPrimaryKeysFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    tc.TABLE_NAME AS {nameof(PrimaryKeyRecord.TableName)},
    CONCAT('PK_', tc.TABLE_NAME) AS {nameof(PrimaryKeyRecord.ConstraintName)},
    kcu.COLUMN_NAME AS {nameof(PrimaryKeyRecord.ColumnName)},
    kcu.ORDINAL_POSITION AS {nameof(PrimaryKeyRecord.ColumnPosition)}
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
    ON kcu.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA
        AND kcu.TABLE_NAME = tc.TABLE_NAME
        AND kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
WHERE tc.CONSTRAINT_SCHEMA = (select DATABASE())
    AND tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
    AND tc.TABLE_NAME != '{DNDBTSysTables.DNDBTDbObjects}';";

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
