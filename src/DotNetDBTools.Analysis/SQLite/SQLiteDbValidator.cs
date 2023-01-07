using System.Collections.Generic;
using System.Text.RegularExpressions;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.SQLite;

internal class SQLiteDbValidator : DbValidator
{
    protected override DbAnalysisContext BuildCurrentAnalysisContext(Database database)
    {
        return new DbAnalysisContext();
    }

    protected override bool DataTypeIsValid(DataType dataType, out string dataTypeErrorMessage)
    {
        dataTypeErrorMessage = null;
        return true;
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
