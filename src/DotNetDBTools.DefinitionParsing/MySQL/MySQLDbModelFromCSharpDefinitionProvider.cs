using System.Collections.Generic;
using System.Reflection;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MySQL;
using DotNetDBTools.DefinitionParsing.Common;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.DefinitionParsing.MySQL;

internal class MySQLDbModelFromCSharpDefinitionProvider : DbModelFromCSharpDefinitionProvider<
    MySQLDatabase,
    MySQLTable,
    MySQLView,
    MySQLIndex,
    MySQLTrigger,
    Models.Core.Column>
{
    public MySQLDbModelFromCSharpDefinitionProvider() : base(
        new MySQLDataTypeMapper(),
        new SpecificDbmsDbObjectCodeMapper(),
        new MySQLDefaultValueMapper())
    {
    }

    protected override void BuildAdditionalDbObjects(Database database, Assembly dbAssembly)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        db.Functions = BuildFunctionModels(dbAssembly);
        db.Functions = new(); // TODO Need to save/read functions from DBMS
    }

    protected override void BuildAdditionalPrimaryKeyModelProperties(Models.Core.PrimaryKey pkModel, BasePrimaryKey pk, string tableName)
    {
        pkModel.Name = $"PK_{tableName}";
    }

    protected override string GetOnUpdateActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.MySQL.ForeignKey)fk).OnUpdate.ToString());
    protected override string GetOnDeleteActionName(BaseForeignKey fk) =>
        MapFKActionNameFromDefinitionToModel(((Definition.MySQL.ForeignKey)fk).OnDelete.ToString());

    private static List<MySQLFunction> BuildFunctionModels(Assembly dbAssembly)
    {
        IEnumerable<IFunction> functions = GetInstancesOfAllTypesImplementingInterface<IFunction>(dbAssembly);
        List<MySQLFunction> functionModels = new();
        foreach (IFunction function in functions)
        {
            MySQLFunction functionModel = new()
            {
                ID = function.DNDBT_OBJECT_ID,
                Name = function.GetType().Name,
                CodePiece = new CodePiece { Code = function.Code.NormalizeLineEndings() },
            };
            functionModels.Add(functionModel);
        }
        return functionModels;
    }
}
