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
        };

        BuildTablesDiff<MySQLTableDiff, ColumnDiff>(dbDiff);
        BuildViewsDiff(dbDiff);
        BuildIndexesDiff(dbDiff);
        BuildTriggersDiff(dbDiff);
        ForeignKeysHelper.BuildUnchangedForeignKeysToRecreateBecauseOfDeps(dbDiff);

        BuildScriptsDiff(dbDiff);
        return dbDiff;
    }

    protected override void BuildAdditionalColumnDiffProperties(ColumnDiff columnDiff, Column newColumn, Column oldColumn)
    {
        if (columnDiff.DataTypeToSet is not null || columnDiff.NotNullToSet is not null)
        {
            columnDiff.DataTypeToSet = newColumn.DataType;
            columnDiff.NotNullToSet = newColumn.NotNull;
            SetDefaultChanged(columnDiff, newColumn, oldColumn);
        }
    }
}
