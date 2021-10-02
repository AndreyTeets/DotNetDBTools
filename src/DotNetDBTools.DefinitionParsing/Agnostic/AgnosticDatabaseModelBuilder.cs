using System.Reflection;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.DefinitionParsing.Agnostic
{
    internal class AgnosticDatabaseModelBuilder : DatabaseModelBuilder
    {
        public AgnosticDatabaseModelBuilder()
            : base(new AgnosticDataTypeMapper(), new AgnosticDefaultValueMapper())
        {
        }

        public AgnosticDatabase BuildDatabaseModel(Assembly dbAssembly)
        {
            return new AgnosticDatabase(DbAssemblyInfoHelper.GetDbName(dbAssembly))
            {
                Tables = BuildTableModels<AgnosticTable>(dbAssembly),
                Views = BuildViewModels<AgnosticView>(dbAssembly),
            };
        }

        protected override string GetOnUpdateActionName(BaseForeignKey fk) => ((Definition.Agnostic.ForeignKey)fk).OnUpdate.ToString();
        protected override string GetOnDeleteActionName(BaseForeignKey fk) => ((Definition.Agnostic.ForeignKey)fk).OnDelete.ToString();
    }
}
