using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Agnostic
{
    internal class AgnosticDbValidator : DbValidator
    {
        public override bool DbIsValid(DatabaseInfo database, out DbError dbError)
        {
            if (!HasNoBadTables(database, out dbError))
                return false;
            return true;
        }
    }
}
