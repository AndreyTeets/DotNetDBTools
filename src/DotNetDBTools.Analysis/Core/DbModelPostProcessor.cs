using System.Linq;
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
        database.Tables = database.Tables.OrderByNameThenByType().ToList();
        database.Views = database.Views.OrderByNameThenByType().ToList();
        database.Scripts = database.Scripts.OrderByNameThenByType().ToList();
        foreach (Table table in database.Tables)
        {
            table.Columns = table.Columns.OrderByNameThenByType().ToList();
            table.UniqueConstraints = table.UniqueConstraints.OrderByNameThenByType().ToList();
            table.CheckConstraints = table.CheckConstraints.OrderByNameThenByType().ToList();
            table.Indexes = table.Indexes.OrderByNameThenByType().ToList();
            table.Triggers = table.Triggers.OrderByNameThenByType().ToList();
            table.ForeignKeys = table.ForeignKeys.OrderByNameThenByType().ToList();
        }
        OrderAdditionalDbObjects(database);
    }
    protected virtual void OrderAdditionalDbObjects(Database database) { }
}
