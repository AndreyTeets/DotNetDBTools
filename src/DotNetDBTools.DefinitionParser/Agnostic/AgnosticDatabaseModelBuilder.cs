using System.Reflection;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.DefinitionParser.Agnostic
{
    internal class AgnosticDatabaseModelBuilder : DatabaseModelBuilder
    {
        public AgnosticDatabaseModelBuilder()
            : base(new AgnosticDataTypeMapper(), new AgnosticDefaultValueMapper())
        {
        }

        public AgnosticDatabaseInfo BuildDatabaseModel(Assembly dbAssembly)
        {
            return new AgnosticDatabaseInfo(DbAssemblyInfoHelper.GetDbName(dbAssembly))
            {
                Tables = BuildTableModels<AgnosticTableInfo>(dbAssembly),
                Views = BuildViewModels<AgnosticViewInfo>(dbAssembly),
            };
        }

        protected override string GetOnUpdateActionName(BaseForeignKey fk) => ((ForeignKey)fk).OnUpdate.ToString();
        protected override string GetOnDeleteActionName(BaseForeignKey fk) => ((ForeignKey)fk).OnDelete.ToString();
    }
}
