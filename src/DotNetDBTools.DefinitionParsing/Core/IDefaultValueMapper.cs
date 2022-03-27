using DotNetDBTools.Definition.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal interface IDefaultValueMapper
{
    public CodePiece MapToDefaultValueModel(IDefaultValue defaultValue);
}
