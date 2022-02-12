using DotNetDBTools.Definition.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal interface IDataTypeMapper
{
    public DataType MapToDataTypeModel(IDataType dataType);
}
