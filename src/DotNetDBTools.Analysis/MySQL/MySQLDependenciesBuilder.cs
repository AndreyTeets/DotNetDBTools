using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.MySQL;

internal class MySQLDependenciesBuilder : DependenciesBuilder
{
    public override void BuildDependencies(Database database)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        Build_Parent_Property_ForAllObjects(db);
        Build_DependsOn_Property_ForAllObjects(db);
    }

    private void Build_Parent_Property_ForAllObjects(Database database)
    {
        Build_Parent_Property_ForTableChildObjects(database);
    }

    private void Build_DependsOn_Property_ForAllObjects(MySQLDatabase database)
    {
        AddForTableObjects();

        void AddForTableObjects()
        {
            foreach (MySQLTable table in database.Tables)
            {
                if (table.PrimaryKey is not null)
                {
                    IEnumerable<Column> referencedColumns = table.Columns.Where(x =>
                        table.PrimaryKey.Columns.Contains(x.Name));

                    HashSet<Column> dependsOn = new();
                    foreach (Column column in referencedColumns)
                        dependsOn.Add(column);
                    table.PrimaryKey.DependsOn = dependsOn.ToList();
                }
                foreach (UniqueConstraint uc in table.UniqueConstraints)
                {
                    IEnumerable<Column> referencedColumns = table.Columns.Where(x =>
                        uc.Columns.Contains(x.Name));

                    HashSet<Column> dependsOn = new();
                    foreach (Column column in referencedColumns)
                        dependsOn.Add(column);
                    uc.DependsOn = dependsOn.ToList();
                }
                foreach (ForeignKey fk in table.ForeignKeys)
                {
                    IEnumerable<Column> referencedColumns = table.Columns.Where(x =>
                        fk.ThisColumnNames.Contains(x.Name));

                    HashSet<Column> dependsOn = new();
                    foreach (Column column in referencedColumns)
                        dependsOn.Add(column);
                    fk.DependsOn = dependsOn.ToList();
                }
                foreach (MySQLIndex index in table.Indexes)
                {
                    IEnumerable<Column> referencedColumns = table.Columns.Where(x =>
                        index.Columns.Contains(x.Name)
                        || index.IncludeColumns.Contains(x.Name));

                    HashSet<Column> dependsOn = new();
                    foreach (Column column in referencedColumns)
                        dependsOn.Add(column);
                    index.DependsOn = dependsOn.ToList();
                }
            }
        }
    }
}
