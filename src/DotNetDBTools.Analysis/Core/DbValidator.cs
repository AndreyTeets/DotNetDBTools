using System.Linq;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    internal abstract class DbValidator
    {
        public abstract bool DbIsValid(Database database, out DbError dbError);

        protected bool HasNoBadTables(Database database, out DbError dbError)
        {
            if (!ForeignKeyReferencesAreValid(database, out dbError))
                return false;
            return true;
        }

        private bool ForeignKeyReferencesAreValid(Database database, out DbError dbError)
        {
            dbError = null;
            foreach (Table table in database.Tables)
            {
                foreach (ForeignKey fk in table.ForeignKeys)
                {
                    if (fk is not null && !database.Tables.Any(x => x.Name == fk.ReferencedTableName))
                    {
                        string errorMessage =
$"Couldn't find table '{fk.ReferencedTableName}' referenced by foreign key '{fk.Name}' in table '{table.Name}'";

                        dbError = new InvalidFKDbError(
                            errorMessage: errorMessage,
                            tableName: table.Name,
                            foreignKeyName: fk.Name,
                            referencedTableName: fk.ReferencedTableName);

                        return false;
                    }
                }
            }
            return true;
        }
    }
}
