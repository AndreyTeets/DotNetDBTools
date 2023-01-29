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
            NewVersion = newDatabase.Version,
            OldVersion = oldDatabase.Version,
        };

        BuildTablesDiff<MySQLTableDiff, MySQLColumnDiff>(dbDiff, newDatabase, oldDatabase);
        BuildViewsDiff(dbDiff, newDatabase, oldDatabase);
        BuildIndexesDiff(dbDiff, newDatabase, oldDatabase);
        BuildTriggersDiff(dbDiff, newDatabase, oldDatabase);
        ForeignKeysHelper.BuildUnchangedForeignKeysToRecreateBecauseOfChangedReferencedObjects(dbDiff, oldDatabase);

        BuildScriptsDiff(dbDiff, newDatabase, oldDatabase);
        return dbDiff;
    }

    protected override void BuildAdditionalColumnDiffProperties(ColumnDiff columnDiff, Column newColumn, Column oldColumn)
    {
        if (columnDiff.DataTypeToSet is not null
            || columnDiff.NotNullToSet is not null
            || columnDiff.IdentityToSet is not null)
        {
            ((MySQLColumnDiff)columnDiff).DefinitionToSet = newColumn;
        }
    }
}
