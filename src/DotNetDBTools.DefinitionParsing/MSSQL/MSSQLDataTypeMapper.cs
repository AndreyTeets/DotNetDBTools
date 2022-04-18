using System;
using DotNetDBTools.Analysis;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.UserDefinedTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.MSSQL;

internal class MSSQLDataTypeMapper : DataTypeMapper
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
                return new AnalysisManager().ConvertDataType(csharpDataType, DatabaseKind.MSSQL);

            case VerbatimDataType verbatimDataType:
                return new DataType { Name = verbatimDataType.Name.ToUpper() };

            case IUserDefinedType:
                return new DataType { Name = dataType.GetType().Name, IsUserDefined = true };

            default:
                throw new InvalidOperationException($"Invalid dataType: {dataType}");
        }
    }
}
