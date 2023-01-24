using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.SQLite;

internal class SQLiteDependenciesBuilder : DependenciesBuilder
{
    public override void BuildDependencies(Database database)
    {
        Build_Parent_Property_ForAllObjects(database);
    }

    private void Build_Parent_Property_ForAllObjects(Database database)
    {
        Build_Parent_Property_ForTableChildObjects(database);
    }
}
