using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo;

internal class MSSQLGetPrimaryKeysFromDBMSSysInfoQuery : GetPrimaryKeysFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    t.name AS [{nameof(PrimaryKeyRecord.TableName)}],
    i.name AS [{nameof(PrimaryKeyRecord.ConstraintName)}],
    c.name AS [{nameof(PrimaryKeyRecord.ColumnName)}],
    ic.key_ordinal AS [{nameof(PrimaryKeyRecord.ColumnPosition)}]
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
    AND t.name NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordMapper Mapper => new MSSQLRecordMapper();

    public class MSSQLRecordMapper : RecordMapper
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
