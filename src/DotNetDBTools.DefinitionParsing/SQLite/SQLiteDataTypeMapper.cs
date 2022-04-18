using System;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.SQLite;

internal class SQLiteDataTypeMapper : DataTypeMapper
{
    public override DataType MapToDataTypeModel(IDataType dataType)
    {
        switch (dataType)
        {
            case null:
                return null;
            case IntDataType:
            case RealDataType:
            case DecimalDataType:
            case BoolDataType:

            case StringDataType:
            case BinaryDataType:
            case GuidDataType:

            case DateDataType:
            case TimeDataType:
            case DateTimeDataType:
                CSharpDataType csharpDataType = CreateCSharpDataTypeModel(dataType);
                return new SQLiteDataTypeConverter().Convert(csharpDataType);

            case VerbatimDataType verbatimDataType:
                return new DataType { Name = verbatimDataType.Name.ToUpper() };

            default:
                throw new InvalidOperationException($"Invalid dataType: {dataType}");
        }
    }
}
