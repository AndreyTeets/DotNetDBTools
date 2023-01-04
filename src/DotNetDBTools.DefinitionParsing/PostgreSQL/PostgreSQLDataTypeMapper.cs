using System;
using DotNetDBTools.Analysis;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.PostgreSQL;

internal class PostgreSQLDataTypeMapper : DataTypeMapper
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
                return new AnalysisManager().ConvertDataType(csharpDataType, DatabaseKind.PostgreSQL);

            case VerbatimDataType verbatimDataType:
                return new DataType { Name = verbatimDataType.Name };

            case ICompositeType:
            case IDomain:
            case IEnumType:
            case IRangeType:
                return new DataType { Name = dataType.GetType().Name };

            // TODO handle user defined base data types (probalby will require declaring their names in definition)
            default:
                throw new InvalidOperationException($"Invalid dataType: {dataType}");
        }
    }
}
