using System.Collections.Generic;
using System.Reflection;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.DefinitionParsing.PostgreSQL
{
    internal class PostgreSQLDatabaseModelBuilder : DatabaseModelBuilder
    {
        public PostgreSQLDatabaseModelBuilder()
            : base(new PostgreSQLDataTypeMapper(), new PostgreSQLDefaultValueMapper())
        {
        }

        public PostgreSQLDatabase BuildDatabaseModel(Assembly dbAssembly)
        {
            return new PostgreSQLDatabase(DbAssemblyInfoHelper.GetDbName(dbAssembly))
            {
                Tables = BuildTableModels<PostgreSQLTable>(dbAssembly),
                Views = BuildViewModels<PostgreSQLView>(dbAssembly),
                Functions = BuildFunctionModels(dbAssembly),
                Procedures = new List<PostgreSQLProcedure>(),
            };
        }

        protected override string GetOnUpdateActionName(BaseForeignKey fk) => ((Definition.PostgreSQL.ForeignKey)fk).OnUpdate.ToString();
        protected override string GetOnDeleteActionName(BaseForeignKey fk) => ((Definition.PostgreSQL.ForeignKey)fk).OnDelete.ToString();

        private static List<PostgreSQLFunction> BuildFunctionModels(Assembly dbAssembly)
        {
            IEnumerable<IFunction> functions = GetInstancesOfAllTypesImplementingInterface<IFunction>(dbAssembly);
            List<PostgreSQLFunction> functionModels = new();
            foreach (IFunction function in functions)
            {
                PostgreSQLFunction functionModel = new()
                {
                    ID = function.ID,
                    Name = function.GetType().Name,
                    Code = function.Code,
                };
                functionModels.Add(functionModel);
            }
            return functionModels;
        }
    }
}
