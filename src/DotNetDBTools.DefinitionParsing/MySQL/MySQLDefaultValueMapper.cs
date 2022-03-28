using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MySQL;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.MySQL;

internal class MySQLDefaultValueMapper : DefaultValueMapper
{
    public override CodePiece MapToDefaultValueModel(IDefaultValue defaultValue)
    {
        if (defaultValue is null)
            return new CodePiece { Code = null };
        if (defaultValue is VerbatimDefaultValue vdv)
            return new CodePiece { Code = vdv.Value };
        return MySQLDefaultValueConverter.ConvertToMySQL(CreateCSharpDefaultValueModel(defaultValue));
    }
}
