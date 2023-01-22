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
            return null;
        if (defaultValue is VerbatimDefaultValue vdv)
            return AgnosticDbObjectCodeMapper.CreateAgnosticCodePiece(vdv.Expression);
        return CreateCSharpDefaultValueModel(defaultValue);
    }
}
