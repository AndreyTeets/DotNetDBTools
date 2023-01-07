using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo;

internal class MySQLGetCheckConstraintsFromDBMSSysInfoQuery : GetCheckConstraintsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    tc.TABLE_NAME AS `{nameof(CheckConstraintRecord.TableName)}`,
    tc.CONSTRAINT_NAME AS `{nameof(CheckConstraintRecord.ConstraintName)}`,
    CONCAT('CHECK ', cc.CHECK_CLAUSE) AS `{nameof(CheckConstraintRecord.ConstraintDefinition)}`
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
INNER JOIN INFORMATION_SCHEMA.CHECK_CONSTRAINTS cc
    ON cc.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA
        AND cc.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
WHERE tc.CONSTRAINT_SCHEMA = (select DATABASE())
    AND tc.CONSTRAINT_TYPE = 'CHECK'
    AND tc.TABLE_NAME NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordMapper Mapper => new MySQLRecordMapper();

    public class MySQLRecordMapper : RecordMapper
    {
        public override CheckConstraint MapToCheckConstraintModel(CheckConstraintRecord ckr)
        {
            return new CheckConstraint()
            {
                ID = Guid.NewGuid(),
                Name = ckr.ConstraintName,
                Expression = new CodePiece { Code = ckr.ConstraintDefinition.ParseOutCheckExpression() },
            };
        }
    }
}
