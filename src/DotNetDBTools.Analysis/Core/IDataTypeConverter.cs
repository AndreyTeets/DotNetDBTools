using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

public interface IDataTypeConverter
{
    DataType Convert(CSharpDataType dataType);
}
