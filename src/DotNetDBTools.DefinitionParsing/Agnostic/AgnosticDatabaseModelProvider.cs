using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.DefinitionParsing.Agnostic;

internal class AgnosticDatabaseModelProvider : DbModelFromCSharpDefinitionProvider<
    AgnosticDatabase,
    AgnosticTable,
    AgnosticView,
    AgnosticIndex,
    AgnosticTrigger,
    Models.Core.Column>
{
    public AgnosticDatabaseModelProvider() : base(
        new AgnosticDataTypeMapper(),
        new AgnosticDbObjectCodeMapper(),
        new AgnosticDefaultValueMapper())
    {
    }

    protected override string GetOnUpdateActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.Agnostic.ForeignKey)fk).OnUpdate.ToString());
    protected override string GetOnDeleteActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.Agnostic.ForeignKey)fk).OnDelete.ToString());
}
