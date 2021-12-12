using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo
{
    internal class MySQLGetUniqueConstraintsFromDBMSSysInfoQuery : GetUniqueConstraintsFromDBMSSysInfoQuery
    {
        public override string Sql =>
$@"SELECT
    tc.TABLE_NAME AS {nameof(UniqueConstraintRecord.TableName)},
    tc.CONSTRAINT_NAME AS {nameof(UniqueConstraintRecord.ConstraintName)},
    kcu.COLUMN_NAME AS {nameof(UniqueConstraintRecord.ColumnName)},
    kcu.ORDINAL_POSITION AS {nameof(UniqueConstraintRecord.ColumnPosition)}
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
    ON kcu.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA
        AND kcu.TABLE_NAME = tc.TABLE_NAME
        AND kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
WHERE tc.CONSTRAINT_SCHEMA = (select DATABASE())
    AND tc.CONSTRAINT_TYPE = 'UNIQUE'
    AND tc.TABLE_NAME != '{DNDBTSysTables.DNDBTDbObjects}';";

        public override RecordMapper Mapper => new MySQLRecordMapper();

        public class MySQLRecordMapper : RecordMapper
        {
            public override UniqueConstraint MapExceptColumnsToUniqueConstraintModel(UniqueConstraintRecord ucr)
            {
                return new UniqueConstraint()
                {
                    ID = Guid.NewGuid(),
                    Name = ucr.ConstraintName,
                };
            }
        }
    }
}
