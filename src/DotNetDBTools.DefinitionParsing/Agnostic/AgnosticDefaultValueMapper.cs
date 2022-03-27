using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Agnostic;

internal class AgnosticDefaultValueMapper : DefaultValueMapper
{
    public override CodePiece MapToDefaultValueModel(IDefaultValue defaultValue)
    {
        if (defaultValue is null)
            return AgnosticDbObjectCodeMapper.CreateAgnosticCodePiece(dk => null);
        else if (defaultValue is Definition.Core.CSharpDefaultValue cdv)
            return CreateCSharpDefaultValueModel(cdv);
        else if (defaultValue is VerbatimDefaultValue vdv)
            return AgnosticDbObjectCodeMapper.CreateAgnosticCodePiece(vdv.Value);
        else
            throw new Exception($"Invalid defaultValue: {defaultValue}");
    }
}
