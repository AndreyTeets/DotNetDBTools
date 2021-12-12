using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.DefinitionParsing.Agnostic
{
    internal class AgnosticDatabaseModelBuilder : DatabaseModelBuilder<
        AgnosticDatabase,
        AgnosticTable,
        AgnosticView,
        Models.Core.Column>
    {
        public AgnosticDatabaseModelBuilder()
            : base(new AgnosticDataTypeMapper(), new AgnosticDefaultValueMapper())
        {
        }

        protected override string GetOnUpdateActionName(BaseForeignKey fk) => ((Definition.Agnostic.ForeignKey)fk).OnUpdate.ToString();
        protected override string GetOnDeleteActionName(BaseForeignKey fk) => ((Definition.Agnostic.ForeignKey)fk).OnDelete.ToString();
    }
}
