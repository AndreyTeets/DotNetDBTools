using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.UserDefinedTypes;
using DotNetDBTools.DefinitionParsing.Common;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.DefinitionParsing.MSSQL;

internal class MSSQLDbModelFromCSharpDefinitionProvider : DbModelFromCSharpDefinitionProvider<
    MSSQLDatabase,
    MSSQLTable,
    MSSQLView,
    MSSQLIndex,
    MSSQLTrigger,
    MSSQLColumn>
{
    public MSSQLDbModelFromCSharpDefinitionProvider() : base(
        new MSSQLDataTypeMapper(),
        new SpecificDbmsDbObjectCodeMapper(),
        new MSSQLDefaultValueMapper())
    {
    }

    protected override void BuildAdditionalDbObjects(Database database, Assembly dbAssembly)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        db.UserDefinedTypes = BuildUserDefinedTypeModels(dbAssembly);
        db.Functions = BuildFunctionModels(dbAssembly);
        db.Functions = new(); // TODO Need to save/read functions from DBMS
    }

    protected override void BuildAdditionalColumnModelProperties(MSSQLColumn columnModel, BaseColumn column, string tableName)
    {
        columnModel.DefaultConstraintName = column.Default is not null
            ? ((Definition.MSSQL.Column)column).DefaultConstraintName ?? $"DF_{tableName}_{columnModel.Name}"
            : null;
    }

    protected override void BuildAdditionalIndexModelProperties(
        Models.Core.Index indexModel, BaseIndex index)
    {
        indexModel.IncludeColumns = ((Definition.MSSQL.Index)index).IncludeColumns?.ToList() ?? new List<string>();
    }

    protected override string GetOnUpdateActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.MSSQL.ForeignKey)fk).OnUpdate.ToString());
    protected override string GetOnDeleteActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.MSSQL.ForeignKey)fk).OnDelete.ToString());

    private List<MSSQLUserDefinedType> BuildUserDefinedTypeModels(Assembly dbAssembly)
    {
        IEnumerable<IUserDefinedType> userDefinedTypes = GetInstancesOfAllTypesImplementingInterface<IUserDefinedType>(dbAssembly);
        List<MSSQLUserDefinedType> userDefinedTypeModels = new();
        foreach (IUserDefinedType udt in userDefinedTypes)
        {
            DataType dataType = DataTypeMapper.MapToDataTypeModel(udt.UnderlyingType);
            MSSQLUserDefinedType udtModel = new()
            {
                ID = udt.DNDBT_OBJECT_ID,
                Name = udt.GetType().Name,
                NotNull = udt.NotNull,
                UnderlyingType = dataType,
            };
            userDefinedTypeModels.Add(udtModel);
        }
        return userDefinedTypeModels;
    }

    private static List<MSSQLFunction> BuildFunctionModels(Assembly dbAssembly)
    {
        IEnumerable<IFunction> functions = GetInstancesOfAllTypesImplementingInterface<IFunction>(dbAssembly);
        List<MSSQLFunction> functionModels = new();
        foreach (IFunction function in functions)
        {
            MSSQLFunction functionModel = new()
            {
                ID = function.DNDBT_OBJECT_ID,
                Name = function.GetType().Name,
                CreateStatement = new CodePiece { Code = function.CreateStatement.NormalizeLineEndings() },
            };
            functionModels.Add(functionModel);
        }
        return functionModels;
    }
}
