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
        MySQLDatabaseDiff databaseDiff = new()
        {
            NewDatabase = newDatabase,
            OldDatabase = oldDatabase,
            ViewsToCreate = new List<MySQLView>(),
            ViewsToDrop = new List<MySQLView>(),
            FunctionsToCreate = new List<MySQLFunction>(),
            FunctionsToDrop = new List<MySQLFunction>(),
            ProceduresToCreate = new List<MySQLProcedure>(),
            ProceduresToDrop = new List<MySQLProcedure>(),
        };

        BuildTablesDiff<MySQLTableDiff>(databaseDiff, newDatabase, oldDatabase);
        IndexesHelper.BuildAllDbIndexesToBeDroppedAndCreated(databaseDiff);
        TriggersHelper.BuildAllDbTriggersToBeDroppedAndCreated(databaseDiff);
        ForeignKeysHelper.BuildAllForeignKeysToBeDroppedAndCreated(databaseDiff);

        BuildViewsDiff(databaseDiff);
        return databaseDiff;
    }
}
