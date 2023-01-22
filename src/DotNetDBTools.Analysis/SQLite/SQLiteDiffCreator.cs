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
        };

        BuildTablesDiff<SQLiteTableDiff, ColumnDiff>(dbDiff);
        BuildIndexesDiff(dbDiff);
        BuildTriggersDiff(dbDiff);

        BuildViewsDiff(dbDiff);
        BuildScriptsDiff(dbDiff);
        return dbDiff;
    }

    protected override void BuildAdditionalTableDiffProperties(TableDiff tableDiff, Table newTable, Table oldTable)
    {
        SQLiteTableDiff tDiff = (SQLiteTableDiff)tableDiff;
        tDiff.NewTable = newTable;
        tDiff.OldTable = oldTable;
    }
}
