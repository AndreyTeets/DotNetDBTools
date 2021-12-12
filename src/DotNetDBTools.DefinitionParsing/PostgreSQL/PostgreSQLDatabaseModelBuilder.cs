using System.Collections.Generic;
using System.Reflection;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.DefinitionParsing.PostgreSQL
{
    internal class PostgreSQLDatabaseModelBuilder : DatabaseModelBuilder<
        PostgreSQLDatabase,
        PostgreSQLTable,
        PostgreSQLView,
        Models.Core.Column>
    {
        public PostgreSQLDatabaseModelBuilder()
            : base(new PostgreSQLDataTypeMapper(), new PostgreSQLDefaultValueMapper())
        {
        }

        protected override void BuildAdditionalDbObjects(Database database, Assembly dbAssembly)
        {
            PostgreSQLDatabase mssqlDatabase = (PostgreSQLDatabase)database;
            mssqlDatabase.Functions = BuildFunctionModels(dbAssembly);
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
