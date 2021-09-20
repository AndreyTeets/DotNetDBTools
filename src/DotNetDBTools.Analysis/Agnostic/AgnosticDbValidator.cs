using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.Analysis.MSSQL
{
    public static class AgnosticDbValidator
    {
        public static bool DbIsValid(AgnosticDatabaseInfo database, out DbError dbError)
        {
            if (!DbValidator.HasNoBadTables(database, out dbError))
                return false;
            return true;
        }
    }
}
