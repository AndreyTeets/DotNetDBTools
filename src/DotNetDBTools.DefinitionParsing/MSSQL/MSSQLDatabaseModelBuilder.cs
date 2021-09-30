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

        public MSSQLDatabaseInfo BuildDatabaseModel(Assembly dbAssembly)
        {
            return new MSSQLDatabaseInfo(DbAssemblyInfoHelper.GetDbName(dbAssembly))
            {
                Tables = BuildTableModels<MSSQLTableInfo>(dbAssembly),
                Views = BuildViewModels<MSSQLViewInfo>(dbAssembly),
                UserDefinedTypes = BuildUserDefinedTypeModels(dbAssembly),
                UserDefinedTableTypes = new List<MSSQLUserDefinedTableTypeInfo>(),
                Functions = BuildFunctionModels(dbAssembly),
                Procedures = new List<MSSQLProcedureInfo>(),
            };
        }

        protected override string GetOnUpdateActionName(BaseForeignKey fk) => ((ForeignKey)fk).OnUpdate.ToString();
        protected override string GetOnDeleteActionName(BaseForeignKey fk) => ((ForeignKey)fk).OnDelete.ToString();

        private List<MSSQLUserDefinedTypeInfo> BuildUserDefinedTypeModels(Assembly dbAssembly)
        {
            IEnumerable<IUserDefinedType> userDefinedTypes = GetInstancesOfAllTypesImplementingInterface<IUserDefinedType>(dbAssembly);
            List<MSSQLUserDefinedTypeInfo> userDefinedTypeInfos = new();
            foreach (IUserDefinedType userDefinedType in userDefinedTypes)
            {
                DataTypeInfo dataTypeInfo = DataTypeMapper.GetDataTypeInfo(userDefinedType.UnderlyingType);
                MSSQLUserDefinedTypeInfo userDefinedTypeInfo = new()
                {
                    ID = userDefinedType.ID,
                    Name = userDefinedType.GetType().Name,
                    Nullable = userDefinedType.Nullable,
                    UnderlyingDataType = dataTypeInfo,
                };
                userDefinedTypeInfos.Add(userDefinedTypeInfo);
            }
            return userDefinedTypeInfos;
        }

        private List<MSSQLFunctionInfo> BuildFunctionModels(Assembly dbAssembly)
        {
            IEnumerable<IFunction> functions = GetInstancesOfAllTypesImplementingInterface<IFunction>(dbAssembly);
            List<MSSQLFunctionInfo> functionInfos = new();
            foreach (IFunction function in functions)
            {
                MSSQLFunctionInfo functionInfo = new()
                {
                    ID = function.ID,
                    Name = function.GetType().Name,
                    Code = function.Code,
                };
                functionInfos.Add(functionInfo);
            }
            return functionInfos;
        }
    }
}
