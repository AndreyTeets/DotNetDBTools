using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.MySQL
{
    internal class MySQLDbValidator : DbValidator
    {
        public override bool DbIsValid(Database database, out DbError dbError)
        {
            if (!HasNoBadTables(database, out dbError))
                return false;
            if (!TriggersCodeIsValid(database, out dbError))
                return false;
            if (!IdentityColumnsAreValid(database, out dbError))
                return false;
            return true;
        }

        private static bool TriggersCodeIsValid(Database database, out DbError dbError)
        {
            dbError = null;
            foreach (Table table in database.Tables)
            {
                foreach (Trigger trigger in table.Triggers)
                {
                    if (!trigger.CodePiece.Code.Contains($"CREATE TRIGGER {trigger.Name} "))
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

        private static bool IdentityColumnsAreValid(Database database, out DbError dbError)
        {
            dbError = null;
            foreach (Table table in database.Tables)
            {
                Column identityColumn = table.Columns.FirstOrDefault(c => c.Identity);
                if (identityColumn is not null && table.PrimaryKey?.Columns.Any(c => c == identityColumn?.Name) != true)
                {
                    string errorMessage =
$"Identity column '{identityColumn.Name}' in table '{table.Name}' is not a primary key";

                    dbError = new InvalidIdentityColumnDbError(
                        errorMessage: errorMessage,
                        tableName: table.Name,
                        identityColumnName: identityColumn.Name);

                    return false;
                }
            }
            return true;
        }
    }
}
