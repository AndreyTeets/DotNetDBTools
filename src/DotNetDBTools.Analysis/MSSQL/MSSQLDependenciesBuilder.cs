using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL;

internal class MSSQLDependenciesBuilder : DependenciesBuilder
{
    public override void BuildDependencies(Database database)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        Build_Parent_Property_ForAllObjects(db);
        Build_DependsOn_Property_ForAllObjects(db);
    }

    private void Build_Parent_Property_ForAllObjects(Database database)
    {
        Build_Parent_Property_ForTableChildObjects(database);
    }

    private void Build_DependsOn_Property_ForAllObjects(MSSQLDatabase database)
    {
        Dictionary<string, DbObject> udtNameToUdtMap = new();
        foreach (DbObject udt in database.UserDefinedTypes)
            udtNameToUdtMap.Add(udt.Name, udt);

        AddForTableObjects();

        void AddForTableObjects()
        {
            foreach (MSSQLTable table in database.Tables)
            {
                foreach (MSSQLColumn column in table.Columns)
                    AddDependencyIfTypeIsUdt(column.DataType.DependsOn, column.DataType.Name);
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
                foreach (MSSQLIndex index in table.Indexes)
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

        void AddDependencyIfTypeIsUdt(List<DbObject> destDeps, string typeName)
        {
            if (udtNameToUdtMap.ContainsKey(typeName))
                destDeps.Add(udtNameToUdtMap[typeName]);
        }
    }
}
