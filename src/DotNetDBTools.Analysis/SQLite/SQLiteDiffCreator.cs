using System.Collections.Generic;
using System.Linq;
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
            NewVersion = newDatabase.Version,
            OldVersion = oldDatabase.Version,
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
        tDiff.NewTableToDefine = newTable;
        tDiff.ChangedColumnsNewNames = GetChangedColumnsNewNames();
        tDiff.ChangedColumnsOldNames = GetChangedColumnsOldNames();

        List<string> GetChangedColumnsNewNames()
        {
            return newTable.Columns.Select(x => x.Name)
                .Except(tableDiff.ColumnsToAdd.Select(x => x.Name))
                .ToList();
        }

        List<string> GetChangedColumnsOldNames()
        {
            return oldTable.Columns.Select(x => x.Name)
                .Except(tableDiff.ColumnsToDrop.Select(x => x.Name))
                .ToList();
        }
    }
}
