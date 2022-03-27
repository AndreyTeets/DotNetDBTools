using DotNetDBTools.Definition.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal abstract class DefaultValueMapper : IDefaultValueMapper
{
    public abstract CodePiece MapToDefaultValueModel(IDefaultValue defaultValue);

    protected Models.Core.CSharpDefaultValue CreateCSharpDefaultValueModel(Definition.Core.CSharpDefaultValue value)
    {
        return new Models.Core.CSharpDefaultValue
        {
            CSharpValue = value.Value,
        };
    }
}
