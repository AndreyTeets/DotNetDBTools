using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo;

internal class MySQLGetPrimaryKeysFromDBMSSysInfoQuery : GetPrimaryKeysFromDBMSSysInfoQuery
{
    public override string Sql =>
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

    public override RecordMapper Mapper => new MySQLRecordMapper();

    public class MySQLRecordMapper : RecordMapper
    {
        public override PrimaryKey MapExceptColumnsToPrimaryKeyModel(PrimaryKeyRecord pkr)
        {
            return new PrimaryKey()
            {
                ID = Guid.NewGuid(),
                Name = pkr.ConstraintName,
            };
        }
    }
}
