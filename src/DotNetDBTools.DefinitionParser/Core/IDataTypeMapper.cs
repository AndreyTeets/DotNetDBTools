using DotNetDBTools.Definition.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParser.Core
{
    internal interface IDataTypeMapper
    {
        public DataTypeInfo GetDataTypeInfo(IDataType dataType);
    }
}
