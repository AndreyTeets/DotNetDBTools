using System.Collections.Generic;
using DotNetDBTools.Analysis.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite;

internal class SQLiteDiffCreator : DiffCreator
{
    public override DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
    {
        SQLiteDatabaseDiff dbDiff = new()
        {
            NewDatabase = newDatabase,
            OldDatabase = oldDatabase,
            ViewsToCreate = new List<View>(),
            ViewsToDrop = new List<View>(),
        };

        BuildTablesDiff<SQLiteTableDiff>(dbDiff);
        IndexesHelper.BuildAllDbIndexesToBeDroppedAndCreated(dbDiff);
        TriggersHelper.BuildAllDbTriggersToBeDroppedAndCreated(dbDiff);

        BuildViewsDiff(dbDiff);
        BuildScriptsDiff(dbDiff);
        return dbDiff;
    }
}
