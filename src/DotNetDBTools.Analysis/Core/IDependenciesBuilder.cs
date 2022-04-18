using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal interface IDependenciesBuilder
{
    public void BuildDependencies(Database database);
}
