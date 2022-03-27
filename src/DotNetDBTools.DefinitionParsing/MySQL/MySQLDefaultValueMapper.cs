using System;
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
        else if (defaultValue is Definition.Core.CSharpDefaultValue cdv)
            return MySQLDefaultValueConverter.ConvertToMySQL(CreateCSharpDefaultValueModel(cdv));
        else if (defaultValue is VerbatimDefaultValue vdv)
            return new CodePiece { Code = vdv.Value };
        else
            throw new Exception($"Invalid defaultValue: {defaultValue}");
    }
}
