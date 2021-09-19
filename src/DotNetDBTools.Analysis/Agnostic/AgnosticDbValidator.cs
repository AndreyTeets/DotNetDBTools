using DotNetDBTools.Analysis.Shared;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.Analysis.MSSQL
{
    public static class AgnosticDbValidator
    {
        public static bool DbIsValid(AgnosticDatabaseInfo database, out string error)
        {
            error = "";
            if (!DbValidator.ForeignKeyReferencesAreValid(database, out string fkError))
            {
                error += fkError;
                return false;
            }

            return true;
        }
    }
}
