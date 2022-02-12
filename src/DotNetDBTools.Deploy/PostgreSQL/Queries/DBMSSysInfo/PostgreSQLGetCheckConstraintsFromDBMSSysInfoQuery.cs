using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetCheckConstraintsFromDBMSSysInfoQuery : GetCheckConstraintsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    t.relname AS ""{nameof(CheckConstraintRecord.TableName)}"",
    c.conname AS ""{nameof(CheckConstraintRecord.ConstraintName)}"",
    pg_catalog.pg_get_constraintdef(c.oid, TRUE) AS ""{nameof(CheckConstraintRecord.ConstraintCode)}""
FROM pg_catalog.pg_class t
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = t.relnamespace
INNER JOIN pg_catalog.pg_constraint c
    ON c.conrelid = t.oid
WHERE t.relkind = 'r'
    AND c.contype = 'c'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog')
    AND t.relname != '{DNDBTSysTables.DNDBTDbObjects}';";

    public override RecordMapper Mapper => new PostgreSQLRecordMapper();

    public class PostgreSQLRecordMapper : RecordMapper
    {
        public override CheckConstraint MapToCheckConstraintModel(CheckConstraintRecord ckr)
        {
            return new CheckConstraint()
            {
                ID = Guid.NewGuid(),
                Name = ckr.ConstraintName,
                CodePiece = new CodePiece { Code = ckr.ConstraintCode },
            };
        }
    }
}
