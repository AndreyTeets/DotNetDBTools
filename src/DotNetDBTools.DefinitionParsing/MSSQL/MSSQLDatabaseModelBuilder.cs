using System.Collections.Generic;
using System.Reflection;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.DefinitionParsing.MSSQL
{
    internal class MSSQLDatabaseModelBuilder : DatabaseModelBuilder
    {
        public MSSQLDatabaseModelBuilder()
            : base(new MSSQLDataTypeMapper(), new MSSQLDefaultValueMapper())
        {
        }

        public MSSQLDatabase BuildDatabaseModel(Assembly dbAssembly)
        {
            return new MSSQLDatabase(DbAssemblyInfoHelper.GetDbName(dbAssembly))
            {
                Tables = BuildTableModels<MSSQLTable>(dbAssembly),
                Views = BuildViewModels<MSSQLView>(dbAssembly),
                UserDefinedTypes = BuildUserDefinedTypeModels(dbAssembly),
                UserDefinedTableTypes = new List<MSSQLUserDefinedTableType>(),
                Functions = BuildFunctionModels(dbAssembly),
                Procedures = new List<MSSQLProcedure>(),
            };
        }

        protected override string GetOnUpdateActionName(BaseForeignKey fk) => ((Definition.MSSQL.ForeignKey)fk).OnUpdate.ToString();
        protected override string GetOnDeleteActionName(BaseForeignKey fk) => ((Definition.MSSQL.ForeignKey)fk).OnDelete.ToString();

        private List<MSSQLUserDefinedType> BuildUserDefinedTypeModels(Assembly dbAssembly)
        {
            IEnumerable<IUserDefinedType> userDefinedTypes = GetInstancesOfAllTypesImplementingInterface<IUserDefinedType>(dbAssembly);
            List<MSSQLUserDefinedType> userDefinedTypeModels = new();
            foreach (IUserDefinedType udt in userDefinedTypes)
            {
                DataType dataType = DataTypeMapper.MapToDataTypeModel(udt.UnderlyingType);
                MSSQLUserDefinedType udtModel = new()
                {
                    ID = udt.ID,
                    Name = udt.GetType().Name,
                    Nullable = udt.Nullable,
                    UnderlyingDataType = dataType,
                };
                userDefinedTypeModels.Add(udtModel);
            }
            return userDefinedTypeModels;
        }

        private List<MSSQLFunction> BuildFunctionModels(Assembly dbAssembly)
        {
            IEnumerable<IFunction> functions = GetInstancesOfAllTypesImplementingInterface<IFunction>(dbAssembly);
            List<MSSQLFunction> functionModels = new();
            foreach (IFunction function in functions)
            {
                MSSQLFunction functionModel = new()
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
