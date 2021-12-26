using System.Collections.Generic;
using System.Reflection;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MySQL;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.DefinitionParsing.MySQL
{
    internal class MySQLDatabaseModelBuilder : DatabaseModelBuilder<
        MySQLDatabase,
        MySQLTable,
        MySQLView,
        Models.Core.Column>
    {
        public MySQLDatabaseModelBuilder() : base(
            new MySQLDataTypeMapper(),
            new MySQLDbObjectCodeMapper(),
            new MySQLDefaultValueMapper())
        {
        }

        protected override void BuildAdditionalDbObjects(Database database, Assembly dbAssembly)
        {
            MySQLDatabase mysqlDatabase = (MySQLDatabase)database;
            mysqlDatabase.Functions = BuildFunctionModels(dbAssembly);
        }

        protected override void BuildAdditionalPrimaryKeyModelProperties(Models.Core.PrimaryKey pkModel, BasePrimaryKey pk, string tableName)
        {
            pkModel.Name = $"PK_{tableName}";
        }

        protected override string GetOnUpdateActionName(BaseForeignKey fk) => ((Definition.MySQL.ForeignKey)fk).OnUpdate.ToString();
        protected override string GetOnDeleteActionName(BaseForeignKey fk) => ((Definition.MySQL.ForeignKey)fk).OnDelete.ToString();

        private static List<MySQLFunction> BuildFunctionModels(Assembly dbAssembly)
        {
            IEnumerable<IFunction> functions = GetInstancesOfAllTypesImplementingInterface<IFunction>(dbAssembly);
            List<MySQLFunction> functionModels = new();
            foreach (IFunction function in functions)
            {
                MySQLFunction functionModel = new()
                {
                    ID = function.ID,
                    Name = function.GetType().Name,
                    CodePiece = new CodePiece { Code = function.Code },
                };
                functionModels.Add(functionModel);
            }
            return functionModels;
        }
    }
}
