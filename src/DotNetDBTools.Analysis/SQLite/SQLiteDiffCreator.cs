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
            NewDatabaseVersion = newDatabase.Version,
            OldDatabaseVersion = oldDatabase.Version,
        };

        BuildTablesDiff<SQLiteTableDiff, ColumnDiff>(dbDiff, newDatabase, oldDatabase);
        BuildIndexesDiff(dbDiff, newDatabase, oldDatabase);
        BuildTriggersDiff(dbDiff, newDatabase, oldDatabase);

        BuildViewsDiff(dbDiff, newDatabase, oldDatabase);
        BuildScriptsDiff(dbDiff, newDatabase, oldDatabase);
        return dbDiff;
    }

    protected override void BuildAdditionalTableDiffProperties(TableDiff tableDiff, Table newTable, Table oldTable)
    {
        SQLiteTableDiff tDiff = (SQLiteTableDiff)tableDiff;
        tDiff.NewTable = newTable;
        tDiff.OldTable = oldTable;
    }
}
