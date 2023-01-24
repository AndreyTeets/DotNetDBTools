using System.Collections.Generic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL;

internal class MSSQLDependenciesBuilder : DependenciesBuilder
{
    public override void BuildDependencies(Database database)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        Build_Parent_Property_ForAllObjects(database);
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
            }
        }

        void AddDependencyIfTypeIsUdt(List<DbObject> destDeps, string typeName)
        {
            if (udtNameToUdtMap.ContainsKey(typeName))
                destDeps.Add(udtNameToUdtMap[typeName]);
        }
    }
}
