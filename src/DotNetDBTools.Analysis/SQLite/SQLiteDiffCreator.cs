using System.Collections.Generic;
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
            ViewsToCreate = new List<SQLiteView>(),
            ViewsToDrop = new List<SQLiteView>(),
        };

        BuildTablesDiff<SQLiteTableDiff>(dbDiff);
        BuildViewsDiff(dbDiff);
        return dbDiff;
    }
}
