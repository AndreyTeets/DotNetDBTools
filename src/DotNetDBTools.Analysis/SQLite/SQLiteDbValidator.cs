using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite
{
    public static class SQLiteDbValidator
    {
        public static bool DbIsValid(SQLiteDatabaseInfo database, out DbError dbError)
        {
            if (!DbValidator.HasNoBadTables(database, out dbError))
                return false;
            return true;
        }

        public static bool CanUpdate(SQLiteDatabaseInfo newDatabase, SQLiteDatabaseInfo oldDatabase, bool allowDataLoss, out string error)
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
