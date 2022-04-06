using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.MySQL;

internal class MySQLDbValidator : DbValidator
{
    public override bool DbIsValid(Database database, out List<DbError> dbErrors)
    {
        dbErrors = new();
        AddCoreDbObjectsErrors(database, dbErrors);
        return dbErrors.Count == 0;
    }

    protected override void AddAdditionalTriggerErrors(Table table, Trigger trigger, List<DbError> dbErrors)
    {
        if (!Regex.IsMatch(trigger.CodePiece.Code, $@"CREATE TRIGGER `?{trigger.Name}`?"))
        {
            string errorMessage =
$"Trigger '{trigger.Name}' in table '{table.Name}' has different name in it's creation code.";

            DbError dbError = new TriggerDbError(
                errorMessage: errorMessage,
                tableName: table.Name,
                triggerName: trigger.Name);

            dbErrors.Add(dbError);
        }
    }

    protected override void AddAdditionalColumnErrors(Table table, Column column, List<DbError> dbErrors)
    {
        if (column.Identity && table.PrimaryKey?.Columns.Any(c => c == column.Name) != true)
        {
            string errorMessage =
$"Identity column '{column.Name}' in table '{table.Name}' is not a primary key";

            DbError dbError = new ColumnDbError(
                errorMessage: errorMessage,
                tableName: table.Name,
                columnName: column.Name);

            dbErrors.Add(dbError);
        }
    }
}
