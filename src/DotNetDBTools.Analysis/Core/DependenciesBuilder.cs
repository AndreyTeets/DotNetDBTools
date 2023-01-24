using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal abstract class DependenciesBuilder : IDependenciesBuilder
{
    public abstract void BuildDependencies(Database database);

    protected void Build_Parent_Property_ForTableChildObjects(Database database)
    {
        foreach (Table table in database.Tables)
        {
            foreach (Column column in table.Columns)
                column.Parent = table;

            if (table.PrimaryKey is not null)
                table.PrimaryKey.Parent = table;
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                uc.Parent = table;
            foreach (CheckConstraint ck in table.CheckConstraints)
                ck.Parent = table;
            foreach (ForeignKey fk in table.ForeignKeys)
                fk.Parent = table;

            foreach (Index index in table.Indexes)
                index.Parent = table;
            foreach (Trigger trigger in table.Triggers)
                trigger.Parent = table;
        }
    }
}
