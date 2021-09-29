using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.MSSQL
{
    internal class MSSQLDbValidator : DbValidator
    {
        public override bool DbIsValid(DatabaseInfo database, out DbError dbError)
        {
            if (!HasNoBadTables(database, out dbError))
                return false;
            if (!TriggersCodeIsValid(database, out dbError))
                return false;
            return true;
        }

        private bool TriggersCodeIsValid(DatabaseInfo database, out DbError dbError)
        {
            dbError = null;
            foreach (TableInfo table in database.Tables)
            {
                foreach (TriggerInfo trigger in table.Triggers)
                {
                    if (!trigger.Code.Contains($"CREATE TRIGGER {trigger.Name} "))
                    {
                        string errorMessage =
$"Trigger '{trigger.Name}' in table '{table.Name}' has different name in it's creation code";

                        dbError = new InvalidTriggerCodeDbError(
                            errorMessage: errorMessage,
                            tableName: table.Name,
                            triggerName: trigger.Name);

                        return false;
                    }
                }
            }
            return true;
        }
    }
}
