using System.Collections.Generic;
using System.Text.RegularExpressions;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.SQLite;

internal class SQLiteDbValidator : DbValidator
{
    public override bool DbIsValid(Database database, out List<DbError> dbErrors)
    {
        dbErrors = new();
        AddCoreDbObjectsErrors(database, dbErrors);
        return dbErrors.Count == 0;
    }

    protected override void AddAdditionalTriggerErrors(Table table, Trigger trigger, List<DbError> dbErrors)
    {
        if (!Regex.IsMatch(trigger.CreateStatement.Code, $@"CREATE TRIGGER \[?{trigger.Name}\]?"))
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
}
