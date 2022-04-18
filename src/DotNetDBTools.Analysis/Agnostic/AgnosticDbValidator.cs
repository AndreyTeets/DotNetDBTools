using System.Collections.Generic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Agnostic;

internal class AgnosticDbValidator : DbValidator
{
    public override bool DbIsValid(Database database, out List<DbError> dbErrors)
    {
        // TODO Convert to all posstible/supported dbms kinds and analyze them?
        dbErrors = new();
        AddCoreDbObjectsErrors(database, dbErrors);
        return dbErrors.Count == 0;
    }
}
