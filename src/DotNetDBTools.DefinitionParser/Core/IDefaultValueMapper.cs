using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.DefinitionParser.Core
{
    internal interface IDefaultValueMapper
    {
        public object MapDefaultValue(BaseColumn column);
    }
}
