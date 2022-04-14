using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

public abstract class DbModelPostProcessor : IDbModelPostProcessor
{
    public void OrderDbObjects(Database database)
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
            OrderAdditionalTableObjects(table);
        }
        OrderAdditionalDbObjects(database);
    }
    protected virtual void OrderAdditionalDbObjects(Database database) { }
    protected virtual void OrderAdditionalTableObjects(Table table) { }
}
