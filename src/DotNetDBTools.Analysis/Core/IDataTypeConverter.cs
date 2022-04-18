using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal interface IDataTypeConverter
{
    public DataType Convert(CSharpDataType dataType);
}
