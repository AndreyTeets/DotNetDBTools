using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Agnostic;

internal class AgnosticDataTypeMapper : DataTypeMapper
{
    public override DataType MapToDataTypeModel(IDataType dataType)
    {
        switch (dataType)
        {
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
                return CreateCSharpDataTypeModel(dataType);

            case VerbatimDataType verbatimDataType:
                return CreateAgnosticVerbatimDataType(verbatimDataType);

            default:
                throw new InvalidOperationException($"Invalid dataType: {dataType}");
        }
    }

    private static DataType CreateAgnosticVerbatimDataType(VerbatimDataType verbatimDataType)
    {
        return new AgnosticVerbatimDataType
        {
            NameCodePiece = AgnosticDbObjectCodeMapper.CreateAgnosticCodePiece(
                dk => verbatimDataType.Name(dk).ToUpper()),
        };
    }
}
