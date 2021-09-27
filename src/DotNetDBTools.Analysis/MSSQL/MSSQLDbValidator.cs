using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL
{
    public static class MSSQLDbValidator
    {
        public static bool DbIsValid(MSSQLDatabaseInfo database, out DbError dbError)
        {
            if (!DbValidator.HasNoBadTables(database, out dbError))
                return false;
            if (!TriggersCodeIsValid(database, out dbError))
                return false;
            return true;
        }

        public static bool CanUpdate(MSSQLDatabaseInfo newDatabase, MSSQLDatabaseInfo oldDatabase, bool allowDataLoss, out string error)
        {
            error = "";
            bool allOldDbTablesExistInNewDb = oldDatabase.Tables.All(oldDbTable =>
                newDatabase.Tables.Any(newDbTable => newDbTable.ID == oldDbTable.ID));

            if (!allowDataLoss && !allOldDbTablesExistInNewDb)
            {
                error += $"New database does not contain all old database tables which will lead to data loss";
                return false;
            }

            return true;
        }

        private static bool TriggersCodeIsValid(DatabaseInfo database, out DbError dbError)
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
