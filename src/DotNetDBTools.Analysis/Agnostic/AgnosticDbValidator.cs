using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Agnostic;

internal class AgnosticDbValidator : DbValidator
{
    public override bool DbIsValid(Database database, out DbError dbError)
    {
        // TODO Convert to all posstible/supported dbms kinds and analyze them?
        if (!HasNoBadTables(database, out dbError))
            return false;
        return true;
    }
}
