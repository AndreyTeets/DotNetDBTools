using System.Collections.Generic;
using DotNetDBTools.Analysis.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.MySQL;

internal class MySQLDiffCreator : DiffCreator
{
    public override DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
    {
        MySQLDatabaseDiff dbDiff = new()
        {
            NewDatabase = newDatabase,
            OldDatabase = oldDatabase,
            ViewsToCreate = new List<View>(),
            ViewsToDrop = new List<View>(),
            FunctionsToCreate = new List<MySQLFunction>(),
            FunctionsToDrop = new List<MySQLFunction>(),
            ProceduresToCreate = new List<MySQLProcedure>(),
            ProceduresToDrop = new List<MySQLProcedure>(),
        };

        BuildTablesDiff<MySQLTableDiff>(dbDiff);
        IndexesHelper.BuildAllDbIndexesToBeDroppedAndCreated(dbDiff);
        TriggersHelper.BuildAllDbTriggersToBeDroppedAndCreated(dbDiff);
        ForeignKeysHelper.BuildAllForeignKeysToBeDroppedAndCreated(dbDiff);

        BuildViewsDiff(dbDiff);
        BuildScriptsDiff(dbDiff);
        return dbDiff;
    }
}
