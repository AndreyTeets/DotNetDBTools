using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal abstract class DbModelPostProcessor : IDbModelPostProcessor
{
    public virtual void DoSpecificDbmsDbModelCreationFromDefinitionPostProcessing(Database database) { }

    public void DoPostProcessing(Database database)
    {
        PostProcessDataTypes(database);
        DoAdditionalPostProcessing(database);
        OrderDbObjectsByName(database);
    }
    protected abstract void PostProcessDataTypes(Database database);
    protected virtual void DoAdditionalPostProcessing(Database database) { }

    private void OrderDbObjectsByName(Database database)
    {
        database.Tables = database.Tables.OrderByName();
        database.Views = database.Views.OrderByName();
        database.Scripts = database.Scripts.OrderByName();
        foreach (Table table in database.Tables)
        {
            table.Columns = table.Columns.OrderByName();
            table.UniqueConstraints = table.UniqueConstraints.OrderByName();
            table.CheckConstraints = table.CheckConstraints.OrderByName();
            table.Indexes = table.Indexes.OrderByName();
            table.Triggers = table.Triggers.OrderByName();
            table.ForeignKeys = table.ForeignKeys.OrderByName();
        }
        OrderAdditionalDbObjects(database);
    }
    protected virtual void OrderAdditionalDbObjects(Database database) { }
}
