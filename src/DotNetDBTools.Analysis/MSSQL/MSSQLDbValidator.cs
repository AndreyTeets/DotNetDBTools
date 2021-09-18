using System.Linq;
using DotNetDBTools.Analysis.Common;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL
{
    public static class MSSQLDbValidator
    {
        public static bool DbIsValid(MSSQLDatabaseInfo database, out string error)
        {
            error = "";
            if (!DbValidator.ForeignKeyReferencesAreValid(database, out string fkError))
            {
                error += fkError;
                return false;
            }

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
    }
}
