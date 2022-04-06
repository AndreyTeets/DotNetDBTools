using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo;

internal class MSSQLGetCheckConstraintsFromDBMSSysInfoQuery : GetCheckConstraintsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    t.name AS [{nameof(CheckConstraintRecord.TableName)}],
    cc.name AS [{nameof(CheckConstraintRecord.ConstraintName)}],
    'CHECK ' + cc.definition AS [{nameof(CheckConstraintRecord.ConstraintCode)}]
FROM sys.tables t
INNER JOIN sys.check_constraints cc
      ON cc.parent_object_id = t.object_id
WHERE t.name NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordMapper Mapper => new MSSQLRecordMapper();

    public class MSSQLRecordMapper : RecordMapper
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
