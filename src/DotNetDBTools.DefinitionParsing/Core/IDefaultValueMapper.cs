using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal interface IDefaultValueMapper
{
    public object MapDefaultValue(BaseColumn column);
}
